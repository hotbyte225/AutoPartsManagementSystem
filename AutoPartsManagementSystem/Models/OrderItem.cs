using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
namespace AutoPartsManagementSystem.Models
{
    public class OrderItem
    {
        public int Id { get; set; }

        [Required]
        public int? OrderId { get; set; }
        public Order? Order { get; set; }

        
        public int ProductId { get; set; }
        public Product? Product { get; set; }

        [Range(1,1000)]
        public int Quantity { get; set; }

        [Precision(18, 2), Range(20000, 100000000)]
        public decimal UnitPrice { get; set; }

    }
}
