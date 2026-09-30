using AutoPartsManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutoPartsManagementSystem.Controllers
{
    [Index(nameof(Product.PartNumber), IsUnique = true)]
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

            if (_db.Products.Any(p => p.PartNumber == product.PartNumber))
            {
                ModelState.AddModelError(nameof(Product.PartNumber), "This part number already exists!");

            }
            if (!ModelState.IsValid)
            {
                return View(product);
            }



            _db.Products.Add(product);
            _db.SaveChanges();
            TempData["Toast"] = "Mahsulot qo'shildi.";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var product = _db.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        [HttpPost,ValidateAntiForgeryToken]
        public IActionResult Edit(int id,Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            } 
            
            if (_db.Products.Any(p => p.PartNumber == product.PartNumber &&  p.Id != product.Id))
            {
                ModelState.AddModelError(nameof(Product.PartNumber), "This part number already exists!");
            }

            if (!ModelState.IsValid)
            {
                
                return View(product);
            }
            _db.Products.Update(product);
            _db.SaveChanges();
            TempData["Toast"] = "Product updated.";
            return RedirectToAction("Index");


        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var product = _db.Products.Find(id);
            if (id != product.Id)
            {
                return NotFound();
            }
            _db.Products.Remove(product);
            _db.SaveChanges();
            TempData["Toast"] = "Product removed.";
            return RedirectToAction("Index");
        }


    }
}
