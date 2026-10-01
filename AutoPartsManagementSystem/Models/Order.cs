using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
namespace AutoPartsManagementSystem.Models
{
    public class Order
    {
        public int Id { get; set; }
        
        public DateTime OrderDate { get; set; }
        
        public int? CustomerId { get; set; }
        public Customer? Customer { get; set; } 
        
        public string? CashierId { get; set; }
        public ApplicationUser? Cashier { get; set; }

        [AllowedValues("Cash", "Card", "Transfer"), StringLength(20)]
        public string PaymentMethod { get; set; } = "Cash";
        
        [Precision(18, 2), Range(0,100)]
        public decimal DiscountPercent { get; set; }
        
        [Precision(18, 2)]
        public decimal Total { get; set; }

        [AllowedValues("Paid", "Pending", "Cancelled"), StringLength(20)]
        public string Status { get; set; } = "Paid";

        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
}
