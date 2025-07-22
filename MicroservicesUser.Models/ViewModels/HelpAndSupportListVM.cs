using MicroservicesUser.Models.DTO;

namespace MicroservicesUser.Models.ViewModels
{
    public class HelpAndSupportListVM : PaginationDTO
    {
        public List<HelpAndSupportVM>? HelpAndSupport { get; set; }
    }
}
