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
