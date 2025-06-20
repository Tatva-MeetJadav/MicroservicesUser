using MicroservicesUser.Models.ViewModels.EmailVerification;

namespace MicroservicesUser.BusinessLogic.Interfaces
{
    public interface IEmailVerificationServices
    {
        Task<EmailVerificationResponseVM> VerifyEmail(EmailVerificationRequestVM requestVM);
    }
}
