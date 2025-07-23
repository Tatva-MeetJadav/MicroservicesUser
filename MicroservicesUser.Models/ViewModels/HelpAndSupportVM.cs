using System.ComponentModel.DataAnnotations;

namespace MicroservicesUser.Models.ViewModels
{
    public class HelpAndSupportVM : ProfileVM
    {
        public string Category { get; set; } = string.Empty;

        [Required(ErrorMessage = "Subject is required.")]
        [RegularExpression(@".*\S+.*", ErrorMessage = "Subject is required.")]
        public string Subject { get; set; } = string.Empty;

        [Required(ErrorMessage = "Message is required.")]
        [RegularExpression(@".*\S+.*", ErrorMessage = "Message is required.")]
        public string Message { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}