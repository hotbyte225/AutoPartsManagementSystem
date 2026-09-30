using AutoPartsManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutoPartsManagementSystem.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _db;
        
        public CategoriesController(ApplicationDbContext db)
        {
            _db = db;
        }

        
        public IActionResult Index()
        {
            return View(_db.Categories.Include(c => c.Products).ToList());
        }

        [HttpPost,ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            category.Name = category.Name?.Trim() ?? "";
            if (_db.Categories.Any(c => c.Name == category.Name))
            {
                TempData["ToastType"] = "error";
                TempData["Toast"] = "This category name already exists!";
                return RedirectToAction("Index");
            }
            if (!ModelState.IsValid)
            {
                TempData["ToastType"] = "error";
                TempData["Toast"] = "Category not added";
                return RedirectToAction("Index");
            }
            _db.Categories.Add(category);
            _db.SaveChanges();
            TempData["Toast"] = "Category added";
            return RedirectToAction("Index");
        }



        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var category = _db.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }
            if (_db.Products.Any(p => p.CategoryId == id))
            {
                TempData["ToastType"] = "error";      
                TempData["Toast"] = "Cannot delete: this category has products.";
                return RedirectToAction("Index");
            }
            _db.Categories.Remove(category);
            _db.SaveChanges();
            
            
            return RedirectToAction("Index");
        }
    }
}
