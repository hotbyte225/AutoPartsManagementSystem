using AutoPartsManagementSystem.Models;
using AutoPartsManagementSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections;



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
        public IActionResult Checkout(CheckoutInput input, int id)
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
            

            
            var ids = input.Items.Select(i => i.ProductId).ToList();
            var products = _db.Products.Where(p => ids.Contains(p.Id)).ToList();

            foreach (var item in input.Items)
            {
                var product = products.FirstOrDefault(p => p.Id == item.ProductId);
                if (product == null)
                {
                    TempData["ToastType"] = "error";
                    TempData["Toast"] = "Product not found";
                    return RedirectToAction("Index");
                }
                if (product.Quantity < item.Quantity)
                {
                    TempData["ToastType"] = "error";
                    TempData["Toast"] = $"Not enough stock for {product.Name}(available: { product.Quantity})";
                    return RedirectToAction("Index");
                }
            }
            

            TempData["Toast"] = "Stock OK";
            return RedirectToAction("Index");
        }
    }
}
