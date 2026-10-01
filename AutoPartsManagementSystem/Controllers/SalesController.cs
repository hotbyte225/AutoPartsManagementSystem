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
    }
}
