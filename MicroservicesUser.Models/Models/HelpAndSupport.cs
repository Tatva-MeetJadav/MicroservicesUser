using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MicroservicesUser.Models.Models
{

    [Table("HelpAndSupport")]
    public class HelpAndSupport
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public string? Subject { get; set; }
        public string? Message { get; set; }
        [StringLength(20)]
        public string? Category { get; set; }
        [Column(TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }
    }
}
