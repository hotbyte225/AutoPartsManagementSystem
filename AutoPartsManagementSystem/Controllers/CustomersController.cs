using AutoPartsManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutoPartsManagementSystem.Controllers
{
    [Authorize(Roles = Roles.Admin + "," + Roles.Manager + "," + Roles.Cashier)]

    public class CustomersController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CustomersController(ApplicationDbContext db)
        {
            _db = db;
        }

        private void Normalize(Customer customer)
        {
            customer.FullName = customer.FullName?.Trim() ?? "";
            customer.Phone = customer.Phone?.Trim() ?? "";
            customer.Email = customer.Email?.Trim();
        }


        public async Task<IActionResult> Index()
        {
             
            return View(await _db.Customers
                .Include(c => c.Orders)
                .OrderBy(c => c.FullName)
                .ToListAsync());
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Customer customer)
        {
            Normalize(customer);
            if (await _db.Customers.AnyAsync(c => c.Phone == customer.Phone))
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
                TempData["ToastType"] = "error";
                TempData["Toast"] = string.Join(" ", errors);
                return RedirectToAction("Index");
            }
            _db.Customers.Add(customer);
            await _db.SaveChangesAsync();
            TempData["Toast"] = "Customer added!";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var customer = await _db.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Customer customer)
        {
            Normalize(customer);

            if (customer.Id != id)
            {
                return NotFound();
            }
            if (await _db.Customers.AnyAsync(c => c.Phone == customer.Phone && c.Id != customer.Id))
            {

                ModelState.AddModelError(nameof(Customer.Phone), "This phone number already exists!");
            }
            if (!ModelState.IsValid)
            {

                return View(customer);
            }
            _db.Customers.Update(customer);
            await _db.SaveChangesAsync();
            TempData["Toast"] = "Customer updated!";
            return RedirectToAction("Index");
        }


        [Authorize(Roles = Roles.Admin + "," + Roles.Manager)]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var customer = await _db.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            if (await _db.Orders.AnyAsync(p => p.CustomerId == id))
            {
                TempData["ToastType"] = "error";
                TempData["Toast"] = "Cannot delete: this customer has orders.";
                return RedirectToAction("Index");
            }
            _db.Customers.Remove(customer);
            await _db.SaveChangesAsync();
            TempData["Toast"] = "Customer removed!";
            return RedirectToAction("Index");
        }
    }
}
