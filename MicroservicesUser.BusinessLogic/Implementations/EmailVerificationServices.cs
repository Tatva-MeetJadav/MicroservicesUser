using MicroservicesUser.BusinessLogic.Interfaces;
using MicroservicesUser.Models.DTO;
using MicroservicesUser.Models.ViewModels;
using MicroservicesUser.Models.ViewModels.EmailVerification;
using Microsoft.Extensions.Configuration;

namespace MicroservicesUser.BusinessLogic.Implementations
{
    public class EmailVerificationServices : IEmailVerificationServices
    {
        private readonly IGenericAPIClientServices _apiClient;
        private readonly IConfiguration _configuration;
        public EmailVerificationServices(IGenericAPIClientServices apiClient, IConfiguration configuration)
        {
            _apiClient = apiClient;
            _configuration = configuration;
        }
        public async Task<EmailVerificationResponseVM> VerifyEmail(EmailVerificationRequestVM requestVM)
        {
            if (!string.IsNullOrEmpty(requestVM?.Email))
            {
                APIResponse<EmailVerificationResponseVM> responseVM = await _apiClient.PostAsync<EmailVerificationRequestVM, APIResponse<EmailVerificationResponseVM>>(requestVM, _configuration["EmailVerificationAPI:Url"] ?? string.Empty);
                return responseVM.Result ?? new EmailVerificationResponseVM();
            }
            else
            {
                throw new Exception("Something went wrong");
            }
        }
    }
}
