using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MicroservicesUser.Models.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [EmailAddress]
        [StringLength(256)]
        public required string Email { get; set; }

        [StringLength(256)]
        public required string Username { get; set; }

        [StringLength(512)]
        public required string PasswordHash { get; set; }

        [StringLength(128)]
        public string? PasswordResetToken { get; set; }
        public DateTime? PasswordResetTokenExpiry { get; set; }
    }
}