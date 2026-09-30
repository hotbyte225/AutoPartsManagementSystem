using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace AutoPartsManagementSystem.Models
{
    [Index(nameof(PartNumber), IsUnique = true)]
    public class Product
    {
        public int Id { get; set; }
        [Required, StringLength(100)]
        public string Name { get; set; } = "";

        [Required, StringLength(50)]
        public string PartNumber { get; set; } = "";

        [StringLength(100)]
        public string? Brand { get; set; }

        public int? CategoryId { get; set; }

        public Category? Category { get; set; }

        [Precision(18, 2), Range(0, 100000000)]
        public decimal Price { get; set; }

        [Range(0, 10000)]
        public int Quantity { get; set; }

        [StringLength(150)]
        public string? Description { get; set; }
    }
}
