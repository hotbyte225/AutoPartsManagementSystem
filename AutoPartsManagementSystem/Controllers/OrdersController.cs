using AutoPartsManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutoPartsManagementSystem.Controllers
{
    [Authorize(Roles = Roles.Admin + "," + Roles.Manager + "," + Roles.Cashier)]

    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _db;
        public OrdersController(ApplicationDbContext db)
        {
            _db = db;
        }
        
        public IActionResult Index()
        {

            return View(_db.Orders
                .Include(o => o.Customer)
                .Include(o => o.Items)
                .OrderByDescending(o => o.OrderDate)
                .ToList());                              
        }

        
        [HttpGet]
        public IActionResult Details(int id)
        {
            var order = _db.Orders
                    .Include(o => o.Customer)
                    .Include(o => o.Cashier)
                    .Include(o => o.Items)
                        .ThenInclude(i => i.Product)

                    .FirstOrDefault(o => o.Id == id);
                    
            if (order == null)
            {
                return NotFound();
            }

            return View(order);

        }
    }
}
