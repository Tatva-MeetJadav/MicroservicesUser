using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using MicroservicesUser.Models.Enums;

namespace MicroservicesUser.Models.Models
{
    public class Admin
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column(TypeName = "varchar(15)")]
        public AdminRole Role { get; set; }

        [EmailAddress]
        [StringLength(256)]
        public required string Email { get; set; }

        [StringLength(128)]
        public required string FirstName { get; set; }

        [StringLength(128)]
        public required string LastName { get; set; }

        [StringLength(10)]
        public string? MobileNumber { get; set; }

        public string? Address { get; set; }

        public string? ProfilePhotoGeneratedName { get; set; }

        [StringLength(512)]
        public required string PasswordHash { get; set; }

        [StringLength(128)]
        public string? PasswordResetToken { get; set; }

        public DateTime? PasswordResetTokenExpiry { get; set; }

    }
}
