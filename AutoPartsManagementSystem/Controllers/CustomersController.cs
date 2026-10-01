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

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var customer = _db.Customers.Find(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(int id,Customer customer)
        {
            customer.FullName = customer.FullName?.Trim() ?? "";
            if (customer.Id != id)
            {
                return NotFound();
            }
            if ((_db.Customers.Any(c => c.Phone == customer.Phone && c.Id != customer.Id)))
            {

                ModelState.AddModelError(nameof(Customer.Phone), "This phone number already exists!");
            }
            if (!ModelState.IsValid)
            {

                return View(customer);
            }
            _db.Customers.Update(customer);
            _db.SaveChanges();
            TempData["Toast"] = "Customer updated!";
            return RedirectToAction("Index");
        }
    }
}
