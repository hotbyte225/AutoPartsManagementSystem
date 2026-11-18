using AutoPartsManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutoPartsManagementSystem.Controllers
{
    [Authorize(Roles = Roles.Admin + "," + Roles.Manager)]
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _db;
        
        public CategoriesController(ApplicationDbContext db)
        {
            _db = db;
        }

        
        public async  Task<IActionResult> Index()
        {
            return View(await _db.Categories.Include(c => c.Products).ToListAsync());
        }

        [HttpPost,ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            category.Name = category.Name?.Trim() ?? "";
            if (await _db.Categories.AnyAsync(c => c.Name == category.Name))
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
            await _db.SaveChangesAsync();
            TempData["Toast"] = "Category added";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _db.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);

        }

        [HttpPost,ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category category)
        {
            category.Name = category.Name?.Trim() ?? "";
            if (id != category.Id)
            {
                return NotFound();
            }
            if ((await _db.Categories.AnyAsync(c => c.Name == category.Name && c.Id != category.Id)))
            {
                
                ModelState.AddModelError(nameof(Category.Name), "This category name already exists!");
            }
            if (!ModelState.IsValid)
            {
           
                return View(category);
            }
            _db.Categories.Update(category);
            await _db.SaveChangesAsync();
            TempData["Toast"] = "Category updated.";
            return RedirectToAction("Index");
        }


        [HttpPost, ValidateAntiForgeryToken]
        public async  Task<IActionResult> Delete(int id)
        {
            var category = await _db.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            if (await _db.Products.AnyAsync(p => p.CategoryId == id))
            {
                TempData["ToastType"] = "error";      
                TempData["Toast"] = "Cannot delete: this category has products.";
                return RedirectToAction("Index");
            }
            _db.Categories.Remove(category);
            await _db.SaveChangesAsync();



            return RedirectToAction("Index");
        }
    }
}
