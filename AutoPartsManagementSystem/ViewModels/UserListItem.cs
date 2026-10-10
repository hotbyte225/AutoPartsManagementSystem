using System.ComponentModel.DataAnnotations.Schema;

namespace AutoPartsManagementSystem.ViewModels
{
    public class UserListItem
    {
        public string Id { get; set; } = "";
        public string FullName { get; set; } = "";
        public string Email { get; set; } = "";
        public string Role { get; set; } = "";
        public bool IsLocked { get; set; }
        [NotMapped]
        public string Initials => string.Concat(
            FullName.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .Take(2)
                    .Select(w => w[0])
        ).ToUpper();
    }
}
