using Microsoft.AspNetCore.Mvc;

namespace AutoPartsManagementSystem.Controllers
{
    public class WarehouseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
