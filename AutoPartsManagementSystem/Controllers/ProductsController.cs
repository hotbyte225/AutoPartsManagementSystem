using AutoPartsManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AutoPartsManagementSystem.Controllers
{
    [Authorize(Roles = Roles.Admin + "," + Roles.Manager + "," + Roles.Cashier)]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ProductsController(ApplicationDbContext db ) 
        {
            _db = db;
        }

        
        public async Task<IActionResult> Index()
        {
            
            return View(await _db.Products.Include(p => p.Category).ToListAsync());
        }

        [Authorize(Roles = Roles.Admin + "," + Roles.Manager)]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            LoadCategoriesAsync();
            return View(new Product());
        }

        [Authorize(Roles = Roles.Admin + "," + Roles.Manager)]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {

            if (await _db.Products.AnyAsync(p => p.PartNumber == product.PartNumber))
            {
                ModelState.AddModelError(nameof(Product.PartNumber), "This part number already exists!");

            }
            if (!ModelState.IsValid)
            {
                LoadCategoriesAsync();
                return View(product);
            }



            _db.Products.Add(product);
            await _db.SaveChangesAsync();
            TempData["Toast"] = "Mahsulot qo'shildi.";
            return RedirectToAction("Index");
        }

        [Authorize(Roles = Roles.Admin + "," + Roles.Manager)]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _db.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            LoadCategoriesAsync();
            return View(product);
        }

        [Authorize(Roles = Roles.Admin + "," + Roles.Manager)]
        [HttpPost,ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            } 
            
            if (await _db.Products.AnyAsync(p => p.PartNumber == product.PartNumber &&  p.Id != product.Id))
            {
                ModelState.AddModelError(nameof(Product.PartNumber), "This part number already exists!");
            }

            if (!ModelState.IsValid)
            {
                LoadCategoriesAsync();
                return View(product);
            }


            try
            {
                _db.Products.Update(product);
                await _db.SaveChangesAsync();
                TempData["Toast"] = "Product updated.";
                return RedirectToAction("Index");

            }
            catch (DbUpdateConcurrencyException)
            {
                
                ModelState.AddModelError("", "This product was modified by someone else. Please reload the page.");
                LoadCategoriesAsync();
                return View(product);
            }
            


        }

        [Authorize(Roles = Roles.Admin + "," + Roles.Manager)]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _db.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            if (await _db.OrderItems.AnyAsync(p => p.ProductId == id))
            {
                TempData["ToastType"] = "error";
                TempData["Toast"] = "Cannot delete: this product has been sold.";
                return RedirectToAction("Index");
            }
            _db.Products.Remove(product);
            await _db.SaveChangesAsync();
            TempData["Toast"] = "Product removed.";
            return RedirectToAction("Index");
        }

        private async Task LoadCategoriesAsync()
        {

            ViewBag.Categories = new SelectList(await _db.Categories.ToListAsync(), "Id", "Name");
        }
    }
}
