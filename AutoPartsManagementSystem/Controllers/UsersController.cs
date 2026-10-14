using AutoPartsManagementSystem.Models;
using AutoPartsManagementSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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


        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var userRole = await _userManager.GetRolesAsync(user);
            var model = new EditUserInput
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email ?? "",
                Role = userRole.FirstOrDefault() ?? ""

            };

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserInput input)
        {
            var user = await _userManager.FindByIdAsync(input.Id);

            if (user == null)
            {
                return NotFound();
            }

            var currentRoles = await _userManager.GetRolesAsync(user);
            var currentRole = currentRoles.FirstOrDefault() ?? "";
            
            if (_userManager.GetUserId(User) == user.Id && input.Role != currentRole)
            {
                ModelState.AddModelError(nameof(input.Role), "You cannot change your own role.");
            }
            var email = input.Email.Trim();

            var existing = await _userManager.FindByEmailAsync(email);
            if (existing != null && existing.Id != user.Id)
            {
                ModelState.AddModelError(nameof(input.Email), "This email is already in use!");
            }
            if (!ModelState.IsValid)
            {
                return View(input);
            }

            user.FullName = input.FullName.Trim();
            user.Email = email;
            user.UserName = email;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(input);
            }
            if (input.Role != currentRole)
            {
                await _userManager.RemoveFromRolesAsync(user, currentRoles);
                await _userManager.AddToRoleAsync(user, input.Role);
            }

            TempData["Toast"] = "User updated!";
            return RedirectToAction("Index");
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Block(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            
            if (user == null)
            {
                return NotFound();
            }

            
            if (_userManager.GetUserId(User) == user.Id)
            {
                TempData["ToastType"] = "error";
                TempData["Toast"] = "You cannot block your accout!";
                return RedirectToAction("Index");
            }


            await _userManager.SetLockoutEnabledAsync(user, true);
            await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
            await _userManager.UpdateSecurityStampAsync(user);

            TempData["Toast"] = "User blocked!";
            return RedirectToAction("Index");
        }

        [HttpPost,ValidateAntiForgeryToken]

        public async Task<IActionResult> Unblock(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            await _userManager.SetLockoutEndDateAsync(user, null);
            await _userManager.ResetAccessFailedCountAsync(user);

            TempData["Toast"] = "User unblocked!";
            return RedirectToAction("Index");
        }

    }
}
