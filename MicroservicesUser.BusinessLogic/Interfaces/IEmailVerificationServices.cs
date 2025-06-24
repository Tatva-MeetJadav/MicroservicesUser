using MicroservicesUser.Models.ViewModels;
using MicroservicesUser.Models.ViewModels.EmailVerification;
using MicroservicesUser.Models.ViewModels.History;

namespace MicroservicesUser.BusinessLogic.Interfaces
{
    public interface IEmailVerificationServices
    {
        Task<EmailVerificationResponseVM> VerifyEmail(EmailVerificationRequestVM requestVM, string token);
        Task<EmailVerificationListHistoryVM> GetEmailVerificationListHistory(PaginationVM paginationVM, string token);
    }
}
