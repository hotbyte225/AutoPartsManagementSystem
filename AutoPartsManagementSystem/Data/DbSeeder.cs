using AutoPartsManagementSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace AutoPartsManagementSystem.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(IServiceProvider services, IConfiguration configuration)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

            string[] roles = { Roles.Admin, Roles.Manager, Roles.Cashier };
            foreach (var roleName in roles)
            {
                bool roleExists = await roleManager.RoleExistsAsync(roleName);
                if (!roleExists)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }


            await EnsureUserAsync(userManager,
                configuration["Seed:AdminEmail"], configuration["Seed:AdminPassword"],
                "Administrator", Roles.Admin);

            await EnsureUserAsync(userManager,
                configuration["Seed:ManagerEmail"], configuration["Seed:ManagerPassword"],
                "Manager", Roles.Manager);


            await EnsureUserAsync(userManager,
                configuration["Seed:CashierEmail"], configuration["Seed:CashierPassword"],
                "Cashier", Roles.Cashier);

            var email = configuration["Seed:AdminEmail"];
            var password = configuration["Seed:AdminPassword"];

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                return;
            }

            var user = await userManager.FindByEmailAsync(email);


            if (user == null)
            {
                user = new ApplicationUser
                {

                    UserName = email,
                    Email = email,
                    FullName = "Administrator"
                };
                var result = await userManager.CreateAsync(user, password);
                if (!result.Succeeded)
                {
                    var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                    throw new Exception($"Admin yaratilmadi: {errors}");
                }
            }
            if (!await userManager.IsInRoleAsync(user, Roles.Admin))
            {
                await userManager.AddToRoleAsync(user, Roles.Admin);
            }

            
        }




        private static async Task EnsureUserAsync(
        UserManager<ApplicationUser> userManager,
        string? email, string? password, string fullName, string role
        )
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                return;
            }
            var user = await userManager.FindByEmailAsync(email);


            if (user == null)
            {
                user = new ApplicationUser
                {

                    UserName = email,
                    Email = email,
                    FullName = role
                };
                var result = await userManager.CreateAsync(user, password);
                if (!result.Succeeded)
                {
                    var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                    throw new Exception($"{role} yaratilmadi: {errors}");
                }
            }
            if (!await userManager.IsInRoleAsync(user, role))
            {
                await userManager.AddToRoleAsync(user, role);
            }
        }
    }
}


