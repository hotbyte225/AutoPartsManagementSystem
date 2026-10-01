using AutoPartsManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace AutoPartsManagementSystem.Controllers
{
    public class CustomersController : Controller
    {
        public readonly ApplicationDbContext _db;
        public CustomersController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View(_db.Customers.ToList());
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(Customer customer)
        {
            customer.FullName = customer.FullName?.Trim() ?? "";
            if (_db.Customers.Any(c => c.Phone == customer.Phone))
            {
                TempData["ToastType"] = "error";
                TempData["Toast"] = "This phone number already exists!";
                return RedirectToAction("Index");
            }
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);
                TempData["Toast"] = string.Join(" ", errors);
                return RedirectToAction("Index");
            }
            _db.Customers.Add(customer);
            _db.SaveChanges();
            TempData["Toast"] = "Customer added!";
            return RedirectToAction("Index");
        }
    }
}
