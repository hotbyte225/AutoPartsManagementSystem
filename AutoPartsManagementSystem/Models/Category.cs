
using System.ComponentModel.DataAnnotations;
namespace AutoPartsManagementSystem.Models
{
    public class Category
    {
        public ICollection<Product> Products { get; set; } = new List<Product>();

        public int Id {  get; set; }

        [Required, StringLength(100)]
        public string Name {  get; set; } = "";

        [StringLength(150)]
        public string? Description {  get; set; }
    }
}
