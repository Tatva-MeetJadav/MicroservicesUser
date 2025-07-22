using MicroservicesUser.Models.DTO;
using MicroservicesUser.Models.ViewModels;

namespace MicroservicesUser.BusinessLogic.Interfaces
{
    public interface IHelpAndSupportServices
    {
        Task<HelpAndSupportVM> GetHelpAndSupportUserData(string token);
        Task<string> AddHelpAndSupport(HelpAndSupportVM helpAndSupportVM, string token);
        Task<HelpAndSupportListVM> GetHelpAndSupportList(HelpAndSupportRequestDTO requestDTO);
    }
}
