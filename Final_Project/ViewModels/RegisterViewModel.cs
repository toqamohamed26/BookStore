using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Final_Project.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [MaxLength(80)]
        [RegularExpression(@"^[^\s]+$", ErrorMessage = "Username cannot contain spaces")]
        public string Name { get; set; }
        [Required]

        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email address")]
        public string Email { get; set; }
        public string ?Photo { get; set; } = "default.png";
        public string Addrss { get; set; }
        [Required]

        public string Role { get; set; }

        [RegularExpression(@"^\d{11}$", ErrorMessage = "Invalid phone number, the phone number should contain 11 digits")]
        public string Phone { get; set; }
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{6,}$", ErrorMessage = "The password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        public List<SelectListItem>? RoleOptions { get; set; }


    }
}
