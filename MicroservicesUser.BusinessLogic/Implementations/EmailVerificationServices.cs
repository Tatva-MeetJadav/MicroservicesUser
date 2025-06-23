using System.Text.Json;
using AutoMapper;
using MicroservicesUser.BusinessLogic.Interfaces;
using MicroservicesUser.DataAccess.Repository.Interfaces;
using MicroservicesUser.Models.DTO;
using MicroservicesUser.Models.Models;
using MicroservicesUser.Models.ViewModels.EmailVerification;
using MicroservicesUser.Models.ViewModels.History;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace MicroservicesUser.BusinessLogic.Implementations
{
    public class EmailVerificationServices : IEmailVerificationServices
    {
        private readonly IGenericAPIClientServices _apiClient;
        private readonly IConfiguration _configuration;
        private readonly IEmailVerificationRepository _emailVerificationRepository;
        private readonly IJwtServices _jwtServices;
        private readonly IMapper _mapper;
        public EmailVerificationServices(IGenericAPIClientServices apiClient, IConfiguration configuration, IEmailVerificationRepository emailVerificationRepository, IJwtServices jwtServices, IMapper mapper)
        {
            _apiClient = apiClient;
            _configuration = configuration;
            _emailVerificationRepository = emailVerificationRepository;
            _jwtServices = jwtServices;
            _mapper = mapper;
        }

        public async Task<EmailVerificationListHistoryVM> GetEmailVerificationListHistory(int page, int pageSize, string searchQuery, string token)
        {
            int id = _jwtServices.GetUserId(token);
            (List<EmailVerification> emailVerifications, int count) = await _emailVerificationRepository.GetListByUserId(id, page, pageSize, searchQuery);

            List<EmailVerificationHistoryVM> emailVerificationHistoryListVMs = emailVerifications.Select(ev =>
            {
                EmailVerificationResponseVM responseVm = JsonConvert.DeserializeObject<EmailVerificationResponseVM>(
                ev.EmailResponseParam.RootElement.GetRawText()) ?? new EmailVerificationResponseVM();

                EmailVerificationRequestVM requestVM = ev?.EmailRequestParam.RootElement.Deserialize<EmailVerificationRequestVM>() ?? new EmailVerificationRequestVM();

                EmailVerificationHistoryVM emailVerificationHistoryVM = new EmailVerificationHistoryVM
                {

                    Valid = responseVm.Valid,
                    Email = requestVM.Email ?? string.Empty,
                    VerifiedAt = ev?.CreatedAt,
                    Deliverability = responseVm.Deliverability ?? string.Empty,
                    OverAllScore = responseVm.OverallScore
                };
                return emailVerificationHistoryVM;
            }).ToList();

            return new EmailVerificationListHistoryVM
            {
                EmailVerificationHistoryListVM = emailVerificationHistoryListVMs,
                PageSize = pageSize,
                CurrentPage = page,
                TotalItems = count
            };
        }

        public async Task<EmailVerificationResponseVM> VerifyEmail(EmailVerificationRequestVM requestVM, string token)
        {
            int id = _jwtServices.GetUserId(token);
            if (!string.IsNullOrEmpty(requestVM?.Email))
            {
                APIResponse<EmailVerificationResponseVM> responseVM = await _apiClient.PostAsync<EmailVerificationRequestVM, APIResponse<EmailVerificationResponseVM>>(requestVM, _configuration["EmailVerificationAPI:Url"] ?? string.Empty);
                EmailVerification emailVerification = new()
                {
                    UserId = id,
                    CreatedAt = DateTime.UtcNow.ToLocalTime(),
                    EmailRequestParam = JsonDocument.Parse(JsonConvert.SerializeObject(requestVM)),
                    EmailResponseParam = JsonDocument.Parse(JsonConvert.SerializeObject(responseVM.Result))
                };
                await _emailVerificationRepository.AddAsync(emailVerification);
                return responseVM.Result ?? new EmailVerificationResponseVM();
            }
            else
            {
                throw new Exception("email is null or empty");
            }
        }
    }
}
