using AutoPartsManagementSystem.Models;
using AutoPartsManagementSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace AutoPartsManagementSystem.Controllers
{
    [Authorize(Roles = Roles.Admin + "," + Roles.Manager + "," + Roles.Cashier)]

    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _db;

        public DashboardController(ApplicationDbContext db)
        {
            _db = db;
        }




        public async  Task<IActionResult> Index()
        {
            DateTime today = DateTime.Today;
            DateTime tomorrow = DateTime.Now.AddDays(1);

            var weekStart = today.AddDays(-6);       
            var prevWeekStart = today.AddDays(-13);  

            var paid = _db.Orders.Where(o => o.Status != "Cancelled");

            var todaySales = await paid
                .Where(o => o.OrderDate >= today && o.OrderDate < tomorrow)
                .SumAsync(o => o.Total);

            var yesterdaySales = await paid
                .Where(o => o.OrderDate >= today.AddDays(-1) && o.OrderDate < today)
                .SumAsync(o => o.Total);

            var weekOrders = await paid
                .CountAsync(o => o.OrderDate >= weekStart && o.OrderDate < tomorrow);

            var prevWeekOrders = await paid
                .CountAsync(o => o.OrderDate >= prevWeekStart && o.OrderDate < weekStart);

            var daily = await paid
                .Where(o => o.OrderDate >= prevWeekStart && o.OrderDate < tomorrow)
                .GroupBy(o => o.OrderDate.Date)
                .Select(g => new { Date = g.Key, Total = g.Sum(o => o.Total) })
                .ToListAsync();

            var chart = new List<DailySalesItem>();
            for (var d = weekStart; d < tomorrow; d = d.AddDays(1))
            {
                chart.Add(new DailySalesItem
                {
                    Day = d.ToString("ddd"),
                    ThisWeek = daily.FirstOrDefault(x => x.Date == d)?.Total ?? 0,
                    LastWeek = daily.FirstOrDefault(x => x.Date == d.AddDays(-7))?.Total ?? 0
                });
            }

            var vm = new DashboardViewModel
            {
                TotalProducts = await _db.Products.CountAsync(),
                CategoriesCount = await _db.Categories.CountAsync(),
                TodaySales = todaySales,
                SalesChangePercent = Percent(todaySales, yesterdaySales),
                WeekOrders = weekOrders,
                OrdersChangePercent = Percent(weekOrders, prevWeekOrders),
                LowStockCount = await _db.Products
                    .CountAsync(p => p.Quantity <= StockRules.LowStockThreshold),
                SalesChart = chart,
                LowStockItems = await _db.Products
                    .Where(p => p.Quantity <= StockRules.LowStockThreshold)
                    .OrderBy(p => p.Quantity)
                    .Take(5)
                    .Select(p => new LowStockItem { Name = p.Name, Quantity = p.Quantity })
                    .ToListAsync(),
                RecentSales = await _db.Orders
                    .OrderByDescending(o => o.OrderDate)
                    .Take(5)
                    .Select(o => new RecentSaleItem
                    {
                        Id = o.Id,
                        CustomerName = o.Customer != null ? o.Customer.FullName : "Walk-in",
                        Total = o.Total,
                        Status = o.Status,
                        OrderDate = o.OrderDate
                    })
                    .ToListAsync()
            };

            return View(vm);
        }

        private static decimal? Percent(decimal current, decimal previous)
        {
            if (previous == 0)
            {
                return null;
            }
            return Math.Round((current - previous) / previous * 100, 1);
        }

      
        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
