using MicroservicesUser.Models.DTO;

namespace MicroservicesUser.Models.ViewModels
{
    public class LogListVM : PaginationDTO
    {
        public List<LogVM>? Logs { get; set; }
    }

    public class LogVM
    {
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