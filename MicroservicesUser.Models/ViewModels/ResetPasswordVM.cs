
using System.ComponentModel.DataAnnotations;

namespace MicroservicesUser.Models.ViewModels
{
    public class ResetPasswordVM
    {
        public string Token { get; set; } = string.Empty;

        [Required(ErrorMessage = "New password is required.")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
        ErrorMessage = "Password should be at least 8 characters long, must contain at least one uppercase letter, one digit, and one special character.")]
        public string NewPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirm password is required.")]
        [Compare(nameof(NewPassword), ErrorMessage = "New password and confirm password should be same.")]
        public string ConfirmPassword { get; set; } = string.Empty;
        public bool IsAdmin { get; set; }
    }
}