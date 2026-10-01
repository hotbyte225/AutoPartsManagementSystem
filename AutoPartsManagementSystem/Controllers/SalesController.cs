using AutoPartsManagementSystem.Models;
using AutoPartsManagementSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;



namespace AutoPartsManagementSystem.Controllers
{
    public class SalesController : Controller
    {
        private readonly ApplicationDbContext _db;

        public SalesController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            var vm = new SaleViewModel();
            vm.Products = _db.Products.OrderBy(p => p.Name).ToList();
            vm.Customers = _db.Customers.OrderBy(c => c.FullName).ToList();
            return View(vm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Checkout(CheckoutInput input)
        {
            if (!input.Items.Any())
            {
                TempData["ToastType"] = "error";
                TempData["Toast"] = "Cart is empty.";
                return RedirectToAction("Index");
            }
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);
                TempData["ToastType"] = "error";
                TempData["Toast"] = string.Join(" ", errors);
                return RedirectToAction("Index");
            }
            TempData["Toast"] = $"Received {input.Items.Count} items";
            return RedirectToAction("Index");
        }
    }
}
