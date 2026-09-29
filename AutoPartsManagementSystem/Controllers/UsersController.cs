using Microsoft.AspNetCore.Mvc;

namespace AutoPartsManagementSystem.Controllers
{
    public class UsersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
