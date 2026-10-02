using AutoPartsManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            
            return View(_db.Products.Include(p => p.Category).ToList());
        }

        [HttpGet]
        public IActionResult Create()
        {
            LoadCategories();
            return View(new Product());
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
                LoadCategories();
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
            LoadCategories();
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
                LoadCategories();
                return View(product);
            }


            try
            {
                _db.Products.Update(product);
                _db.SaveChanges();
                TempData["Toast"] = "Product updated.";
                return RedirectToAction("Index");

            }
            catch (DbUpdateConcurrencyException)
            {
                
                ModelState.AddModelError("", "This product was modified by someone else. Please reload the page.");
                LoadCategories();
                return View(product);
            }
            


        }
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var product = _db.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            if (_db.OrderItems.Any(p => p.ProductId == id))
            {
                TempData["ToastType"] = "error";
                TempData["Toast"] = "Cannot delete: this product has been sold.";
                return RedirectToAction("Index");
            }
            _db.Products.Remove(product);
            _db.SaveChanges();
            TempData["Toast"] = "Product removed.";
            return RedirectToAction("Index");
        }

        private void LoadCategories()
        {

            ViewBag.Categories = new SelectList(_db.Categories.ToList(), "Id", "Name");
        }
    }
}
