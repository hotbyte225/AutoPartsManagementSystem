using AutoPartsManagementSystem.Models;
using System.ComponentModel.DataAnnotations;


namespace AutoPartsManagementSystem.ViewModels
{
    public class CreateUserInput
    {
        [Required, StringLength(100)]
        public string FullName { get; set; } = "";

        [Required, EmailAddress,StringLength(100)]
        public string Email { get; set; } = "";

        [Required,StringLength(20)]
        public string Password { get; set; } = "";

        [AllowedValues(Roles.Admin, Roles.Manager, Roles.Cashier)]
        public string Role { get; set; } = Roles.Cashier;
    }
}
