using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MicroservicesUser.Models.Models
{
    public class Notification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int UserId { get; set; }

        public int? HelpAndSupportId { get; set; }

        public HelpAndSupport? HelpAndSupport { get; set; }

        public User? User { get; set; }

        public bool IsRead { get; set; }

        [Column(TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }

    }
}
