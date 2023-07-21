using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Final_Project.ViewModels
{
    public class UsersDetailsViewModel
    {
        public string id { get; set; }
        [Required]
        [MaxLength(80)]
        [RegularExpression(@"^[^\s]+$", ErrorMessage = "Username cannot contain spaces")]
        public string Name { get; set; }
        [Required]

        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email address")]
        public string Email { get; set; }
        public string? Photo { get; set; } = "default.png";
        public string Addrss { get; set; }
        [Required]

        public string Role { get; set; }

        [RegularExpression(@"^\d{11}$", ErrorMessage = "Invalid phone number, the phone number should contain 11 digits")]
        public string Phone { get; set; }

        public List<SelectListItem>? RoleOptions { get; set; }
    }
}
