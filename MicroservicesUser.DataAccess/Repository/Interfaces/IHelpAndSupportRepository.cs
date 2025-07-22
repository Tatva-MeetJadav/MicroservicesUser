using MicroservicesUser.Models.DTO;
using MicroservicesUser.Models.Models;

namespace MicroservicesUser.DataAccess.Repository.Interfaces
{
    public interface IHelpAndSupportRepository
    {
        Task AddAsync(HelpAndSupport helpAndSupport);
        Task<(List<HelpAndSupport>, int)> GetListAsync(HelpAndSupportRequestDTO requestDTO);
    }
}