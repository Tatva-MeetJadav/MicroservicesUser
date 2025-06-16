using System.ComponentModel.DataAnnotations;

namespace MicroservicesUser.Models.ViewModels
{
    public class RegisterVM
    {
        [Required(ErrorMessage = "Email is required.")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Email is not valid.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
        ErrorMessage = "Password should be at least 8 characters long, must contain at least one uppercase letter, one digit, and one special character.")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "ConfirmPassword is required.")]
        [Compare(nameof(Password), ErrorMessage = "Password and ConfirmPassword should be same.")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Username is required.")]
        [RegularExpression(@"^(?![\s\-\(\)\[\]&'/\+,]*$)(?=.*[A-Za-z0-9À-ÿ])([A-Za-z0-9À-ÿ\s\-\(\)\[\]&'/\+,.]+)$", ErrorMessage = "Username is not valid.")]
        public string Username { get; set; } = string.Empty;
    }
}
