namespace AutoPartsManagementSystem.ViewModels
{

    public class DashboardViewModel
    {
        public int TotalProducts { get; set; }
        public int CategoriesCount { get; set; }
        public decimal TodaySales { get; set; }
        public decimal? SalesChangePercent { get; set; }
        public int WeekOrders { get; set; }
        public decimal? OrdersChangePercent { get; set; }
        public int LowStockCount { get; set; }

        public List<DailySalesItem> SalesChart { get; set; } = new();
        public List<LowStockItem> LowStockItems { get; set; } = new();
        public List<RecentSaleItem> RecentSales { get; set; } = new();
    }


    public class DailySalesItem
    {
        public string Day { get; set; } = "";
        public decimal ThisWeek { get; set; }
        public decimal LastWeek { get; set; }
    }

    public class LowStockItem
    {
        public string Name { get; set; } = "";
        public int Quantity { get; set; }
    }

  
    public class RecentSaleItem
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = "";
        public decimal Total { get; set; }
        public string Status { get; set; } = "";
        public DateTime OrderDate { get; set; }
    }
}