

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoPartsManagementSystem.Models
{
    [Index(nameof(Phone), IsUnique = true)]
    public class Customer 
    {
        public int Id { get; set; }
        public ICollection<Order> Orders { get; set; } = new List<Order>();

        
        [Required, StringLength(100)]
        public string FullName { get; set; } = "";

        [Required, StringLength(20)]
        public string Phone { get; set; } = "";
        
        [EmailAddress(ErrorMessage = "Invalid email address format."),StringLength(100)]
        public string? Email { get; set; }

        [AllowedValues("Individual", "Company"), StringLength(20)]
        public string Type { get; set; } = "Individual";
        [NotMapped]
        public string Initials => string.Concat(
            FullName.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .Take(2)
                    .Select(w => w[0])
        ).ToUpper();
    }
}
