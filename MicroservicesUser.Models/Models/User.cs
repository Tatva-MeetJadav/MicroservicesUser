using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MicroservicesUser.Models.Models
{
    [Table("Users")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public bool IsBlocked { get; set; }

        [EmailAddress]
        [StringLength(256)]
        public required string Email { get; set; }

        [StringLength(512)]
        public required string PasswordHash { get; set; }

        [StringLength(128)]
        public string? PasswordResetToken { get; set; }
        public DateTime? PasswordResetTokenExpiry { get; set; }

        [StringLength(10)]
        public string? MobileNumber { get; set; }

        public string? Address { get; set; }

        [StringLength(128)]
        public required string FirstName { get; set; }

        [StringLength(128)]
        public required string LastName { get; set; }

        public string? ProfilePhotoGeneratedName { get; set; }


    }
}