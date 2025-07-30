using MicroservicesUser.Models.DTO;
using MicroservicesUser.Models.Models;

namespace MicroservicesUser.DataAccess.Repository.Interfaces
{
    public interface ILogRepository
    {
        Task<(List<Log>, int)> GetListAsync(PaginationDTO paginationDTO);
        Task<List<string>> GetSuggestionMessagesList(string searchQuery);

    }
}