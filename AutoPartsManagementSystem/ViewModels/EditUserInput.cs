using System.ComponentModel.DataAnnotations;
using AutoPartsManagementSystem.Models;


namespace AutoPartsManagementSystem.ViewModels
{
    public class EditUserInput
    {
        public string Id { get; set; } = "";

        [Required, StringLength(100)]
        public string FullName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [AllowedValues(Roles.Admin, Roles.Manager, Roles.Cashier)]
        public string Role { get; set; }
    }
}
