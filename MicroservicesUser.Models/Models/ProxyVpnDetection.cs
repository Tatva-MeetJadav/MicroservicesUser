using MicroservicesUser.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace MicroservicesUser.Models.Models
{
    public class ProxyVpnDetection
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int UserId { get; set; }

        [Column(TypeName = "varchar(20)")]
        public Status Status { get; set; }

        public required JsonDocument ProxyVpnRequestParam { get; set; }

        public required JsonDocument ProxyVpnResponseParam { get; set; }

        [Column(TypeName = "timestamp without time zone")]
        public required DateTime CreatedAt { get; set; }

        public User? User { get; set; }

    }
}
