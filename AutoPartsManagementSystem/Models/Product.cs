using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace AutoPartsManagementSystem.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required, StringLength(150)]
        public string Name { get; set; } = "";
        [Required, StringLength(150)]
        public string PartNumber { get; set; } = "";
        public string? Brand { get; set; }
        public string? Category { get; set; }
        [Precision(18, 2)]
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        [StringLength(150)]
        public string? Description { get; set; }
    }
}
