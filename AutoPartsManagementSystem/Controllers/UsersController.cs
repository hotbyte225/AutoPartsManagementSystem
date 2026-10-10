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
    }
}
