using System.ComponentModel.DataAnnotations;

namespace MicroservicesUser.Models.ViewModels
{
    public class ProfileVM : RegisterVM
    {
        public int Id { get; set; }

        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be exactly 10 digits and contain only numbers.")]
        [Required(ErrorMessage = "Phone number is required.")]
        public string PhoneNumber { get; set; } = string.Empty;

        public string? Address { get; set; } = string.Empty;

        public string ProfilePhotoGeneratedName { get; set; } = string.Empty;

        public string ProfilePhotoName { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;
    }
}