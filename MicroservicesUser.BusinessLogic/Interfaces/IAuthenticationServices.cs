using MicroservicesUser.Models.ViewModels;

namespace MicroservicesUser.BusinessLogic.Interfaces
{
    public interface IAuthenticationServices
    {
        Task<string> RegisterUser(RegisterVM registerVM);
        Task<string> LoginUser(LoginVM loginVM);
        Task<string> ForgotPassword(string email);
        Task<string> ValidatePasswordResetToken(string token);
        Task<string> ResetPassword(ResetPasswordVM resetPasswordVM);
    }
}
