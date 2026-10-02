using AutoPartsManagementSystem.Models;
using AutoPartsManagementSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;



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
            input.Items = input.Items
                .GroupBy(i => i.ProductId)
                .Select(g => new CheckoutItemInput
                {
                    ProductId = g.Key,
                    Quantity = g.Sum(i => i.Quantity)
                })
                .ToList();


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
                    TempData["Toast"] = $"Not enough stock for {product.Name} (available: {product.Quantity})";
                    return RedirectToAction("Index");

                }
            }

            if (input.CustomerId.HasValue && !_db.Customers.Any(c => c.Id == input.CustomerId))
            {
                TempData["ToastType"] = "error";
                TempData["Toast"] = "Customer not found";
                return RedirectToAction("Index");
            }
            var order = new Order
            {
                OrderDate = DateTime.Now,
                CustomerId = input.CustomerId,
                PaymentMethod = input.PaymentMethod,
                DiscountPercent = input.DiscountPercent,
                Status = "Paid",
                CashierId = User.FindFirstValue(ClaimTypes.NameIdentifier)
            };

            foreach (var item in input.Items)
            {

                var product = products.FirstOrDefault(p => p.Id == item.ProductId);
                order.Items.Add(new OrderItem
                {
                    ProductId = product.Id,
                    Quantity = item.Quantity,
                    UnitPrice = product.Price
                });
                product.Quantity -= item.Quantity;
            }




            decimal subtotal = order.Items.Sum(i => i.UnitPrice * i.Quantity);
            decimal discount = subtotal * input.DiscountPercent / 100;
            order.Total = subtotal - discount;


            try
            {
                _db.Orders.Add(order);
                _db.SaveChanges();
                
            }
            catch (DbUpdateConcurrencyException)
            {
                TempData["ToastType"] = "error";
                TempData["Toast"] = "Stock was changed by another sale. Please try again.";
                return RedirectToAction("Index");
            }
            

            TempData["Toast"] = $"Sale completed: #INV-{order.Id}";
            return RedirectToAction("Index");
        }
    }
}
