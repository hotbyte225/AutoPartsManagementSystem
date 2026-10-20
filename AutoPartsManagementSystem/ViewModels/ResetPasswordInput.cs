using System.ComponentModel.DataAnnotations;

namespace AutoPartsManagementSystem.ViewModels
{
    public class ResetPasswordInput
    {
        public string Id { get; set; }

        [Required]
        public string NewPassword { get; set; }


    }
}
