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

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }
            _db.Products.Add(product);
            _db.SaveChanges();
            TempData["Toast"] = "Mahsulot qo'shildi.";
            return RedirectToAction("Index");
        }

        


    }
}
