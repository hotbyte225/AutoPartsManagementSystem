
using Microsoft.AspNetCore.Identity;

namespace AutoPartsManagementSystem.Models
{
    public class ApplicationUser : IdentityUser
    {

        public string FullName { get; set; }
      
    }
}
