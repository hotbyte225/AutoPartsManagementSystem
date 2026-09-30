using AutoPartsManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutoPartsManagementSystem.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ProductsController(ApplicationDbContext db ) 
        {
            _db = db;
        }


        public IActionResult Index()
        {
            return View(_db.Products.ToList());
        }
        public IActionResult Create()
        {
            return View();
        }
    }
}
