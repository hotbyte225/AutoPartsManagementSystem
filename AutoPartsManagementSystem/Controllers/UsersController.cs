using AutoPartsManagementSystem.Models;
using AutoPartsManagementSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutoPartsManagementSystem.Controllers
{
    [Authorize(Roles = Roles.Admin)]

    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        
        
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.OrderBy(u => u.FullName).ToListAsync();
            List<UserListItem> userList = new List<UserListItem> ();
            
            foreach (var user in users)
            {
                var userRole = await _userManager.GetRolesAsync(user);
                var isLocked = await _userManager.IsLockedOutAsync(user);
                userList.Add(new UserListItem
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Email = user.Email ?? "",
                    Role = userRole.FirstOrDefault() ?? "-",
                    IsLocked = isLocked,
                    
                    
                });

            }
            return View(userList);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserInput input)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);
                TempData["ToastType"] = "error";
                TempData["Toast"] = string.Join(" ", errors);
                return RedirectToAction("Index");
            }
            var email = input.Email.Trim();

            var existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser != null)
            {
                TempData["ToastData"] = "error";
                TempData["Toast"] = "This email is already exists";
                return RedirectToAction("Index");
            }

            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                FullName = input.FullName.Trim()
            };
            var result = await _userManager.CreateAsync(user, input.Password);

            if (!result.Succeeded)
            {
                TempData["ToastType"] = "error";
                TempData["Toast"] = string.Join(" ", result.Errors.Select(e => e.Description));
                return RedirectToAction("Index");
            }

            await _userManager.AddToRoleAsync(user, input.Role);
            TempData["Toast"] = $"User {user.FullName} created.";
            return RedirectToAction("Index");
        }
    }
}
