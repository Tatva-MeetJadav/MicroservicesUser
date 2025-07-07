using MicroservicesUser.Models.ViewModels;

namespace MicroservicesUser.BusinessLogic.Interfaces
{
    public interface IAuthenticationServices
    {
        Task<string> RegisterUser(RegisterVM registerVM);
        Task<string> LoginUser(LoginVM loginVM, string ipAddress);
        Task<string> ForgotPassword(LoginVM loginVM);
        Task<string> ValidatePasswordResetToken(string token, bool isAdmin);
        Task<string> ResetPassword(ResetPasswordVM resetPasswordVM);
        void AddInMemoryToken(string token);
    }
}
