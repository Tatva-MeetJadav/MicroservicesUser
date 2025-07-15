using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MicroservicesUser.Models.Models
{

    [Table("Logs")]
    public class Log
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string? Message { get; set; }

        public string? MessageTemplate { get; set; }

        public int Level { get; set; }

        public DateTime RaiseDate { get; set; }

        public string? Exception { get; set; }

        public string? Properties { get; set; }

        public string? PropsTest { get; set; }

        public string? MachineName { get; set; }
    }

}