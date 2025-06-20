using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;


namespace MicroservicesUser.Models.Models
{
    public class EmailVerification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public required JsonDocument EmailRequestParam { get; set; }

        public required JsonDocument EmailResponseParam { get; set; }

        public required User User { get; set; }

        public int UserId { get; set; }
    }
}

