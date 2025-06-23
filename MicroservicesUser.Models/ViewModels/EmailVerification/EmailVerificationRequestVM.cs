using System.ComponentModel.DataAnnotations;

namespace MicroservicesUser.Models.ViewModels.EmailVerification
{
    public class EmailVerificationRequestVM
    {
        [Required(ErrorMessage = "Email is required.")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Email is not valid.")]
        public string? Email { get; set; }
    }
}
