using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using MicroservicesUser.Models.Enums;


namespace MicroservicesUser.Models.Models
{
    public class EmailVerification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public required JsonDocument EmailRequestParam { get; set; }

        public required JsonDocument EmailResponseParam { get; set; }

        [Column(TypeName = "timestamp without time zone")]
        public required DateTime CreatedAt { get; set; }

        public User? User { get; set; }

        public int UserId { get; set; }

        [Column(TypeName = "varchar(20)")]
        public Status Status { get; set; }
        
        public bool IsDeleted { get; set; }
    }
}

