using AutoPartsManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoPartsManagementSystem.Controllers
{
    [Authorize(Roles = Roles.Admin + "," + Roles.Manager)]

    public class ReportsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
