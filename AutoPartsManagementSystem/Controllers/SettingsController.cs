using AutoPartsManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoPartsManagementSystem.Controllers
{
    [Authorize(Roles = Roles.Admin)]

    public class SettingsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
