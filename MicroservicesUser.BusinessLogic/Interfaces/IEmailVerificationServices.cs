using MicroservicesUser.Models.DTO;
using MicroservicesUser.Models.ViewModels.EmailVerification;
using MicroservicesUser.Models.ViewModels.History;

namespace MicroservicesUser.BusinessLogic.Interfaces
{
    public interface IEmailVerificationServices
    {
        Task<EmailVerificationResponseVM> VerifyEmail(EmailVerificationRequestVM requestVM, string token);
        Task<EmailVerificationListHistoryVM> GetEmailVerificationListHistory(PaginationDTO paginationDTO, string token);
        Task<EmailVerificationDetailVM> GetEmailDetailedHistory(string id);
    }
}
