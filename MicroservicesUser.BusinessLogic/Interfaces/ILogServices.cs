using MicroservicesUser.Models.DTO;
using MicroservicesUser.Models.ViewModels;

namespace MicroservicesUser.BusinessLogic.Interfaces
{
    public interface ILogServices
    {
        Task<LogListVM> GetLogList(PaginationDTO paginationDTO);
        Task<List<string>> GetSuggestionMessagesList(string searchQuery);
    }
}
