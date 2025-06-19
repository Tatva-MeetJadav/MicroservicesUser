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

        [Required(ErrorMessage = "Confirm password is required.")]
        [Compare(nameof(Password), ErrorMessage = "Password and confirm password should be same.")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "First name is required.")]
        [RegularExpression(@"^(?![\s\-\(\)\[\]&'/\+,]*$)(?=.*[A-Za-z0-9À-ÿ])([A-Za-z0-9À-ÿ\s\-\(\)\[\]&'/\+,.]+)$", ErrorMessage = "First name is not valid.")]
        public string FirstName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Last name is required.")]
        [RegularExpression(@"^(?![\s\-\(\)\[\]&'/\+,]*$)(?=.*[A-Za-z0-9À-ÿ])([A-Za-z0-9À-ÿ\s\-\(\)\[\]&'/\+,.]+)$", ErrorMessage = "Last name is not valid.")]
        public string LastName { get; set; } = string.Empty;
    }
}
