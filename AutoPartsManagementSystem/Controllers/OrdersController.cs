using Microsoft.AspNetCore.Mvc;

namespace AutoPartsManagementSystem.Controllers
{
    public class OrdersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
