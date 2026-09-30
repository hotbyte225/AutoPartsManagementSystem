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
    }
}
