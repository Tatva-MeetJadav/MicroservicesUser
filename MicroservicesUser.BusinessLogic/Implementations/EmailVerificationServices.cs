using System.Text.Json;
using MicroservicesUser.BusinessLogic.Interfaces;
using MicroservicesUser.Common.ResourcesFiles;
using MicroservicesUser.DataAccess.Repository.Interfaces;
using MicroservicesUser.Models.DTO;
using MicroservicesUser.Models.Enums;
using MicroservicesUser.Models.Models;
using MicroservicesUser.Models.ViewModels;
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
        private readonly IEncryptDecryptServices _encryptDecryptServices;

        public EmailVerificationServices(IGenericAPIClientServices apiClient, IConfiguration configuration, IEmailVerificationRepository emailVerificationRepository, IJwtServices jwtServices, IEncryptDecryptServices encryptDecryptServices)
        {
            _apiClient = apiClient;
            _configuration = configuration;
            _emailVerificationRepository = emailVerificationRepository;
            _jwtServices = jwtServices;
            _encryptDecryptServices = encryptDecryptServices;
        }

        public async Task<EmailVerificationListHistoryVM> GetEmailVerificationListHistory(PaginationVM paginationVM, string token)
        {
            int id = _jwtServices.GetUserId(token);
            byte[] key = Convert.FromBase64String(_configuration["EncryptId:Key"] ?? string.Empty);
            byte[] iv = Convert.FromBase64String(_configuration["EncryptId:IV"] ?? string.Empty);
            (List<EmailVerification> emailVerifications, int count) = await _emailVerificationRepository.GetListByUserId(id, paginationVM);

            List<EmailVerificationHistoryVM> emailVerificationHistoryListVMs = emailVerifications.Select(ev =>
            {
                EmailVerificationResponseVM responseVM = JsonConvert.DeserializeObject<EmailVerificationResponseVM>(
                ev.EmailResponseParam.RootElement.GetRawText()) ?? new EmailVerificationResponseVM();

                EmailVerificationRequestVM requestVM = ev?.EmailRequestParam.RootElement.Deserialize<EmailVerificationRequestVM>() ?? new EmailVerificationRequestVM();
                EmailVerificationHistoryVM emailVerificationHistoryVM = new()
                {
                    Id = _encryptDecryptServices.EncryptId(ev?.Id ?? 0),
                    Valid = responseVM.Valid,
                    Email = requestVM.Email ?? string.Empty,
                    VerifiedAt = ev!.CreatedAt,
                    Deliverability = responseVM.Deliverability ?? string.Empty,
                    OverAllScore = responseVM.OverallScore,
                    ResponseStatus = ev?.Status.ToString() ?? string.Empty,
                };
                return emailVerificationHistoryVM;
            }).ToList();

            return new EmailVerificationListHistoryVM
            {
                EmailVerificationHistoryListVM = emailVerificationHistoryListVMs,
                PageSize = paginationVM.PageSize,
                CurrentPage = paginationVM.CurrentPage,
                TotalItems = count
            };
        }

        public async Task<EmailVerificationResponseVM> VerifyEmail(EmailVerificationRequestVM requestVM, string token)
        {
            int id = _jwtServices.GetUserId(token);
            if (!string.IsNullOrEmpty(requestVM?.Email))
            {
                APIResponseDTO<EmailVerificationResponseVM> responseVM;
                EmailVerification emailVerification;
                try
                {
                    responseVM = await _apiClient.PostAsync<EmailVerificationRequestVM, APIResponseDTO<EmailVerificationResponseVM>>(requestVM, _configuration["EmailVerificationAPI:Url"] ?? string.Empty);
                }
                catch (Exception ex)
                {
                    emailVerification = new()
                    {
                        UserId = id,
                        CreatedAt = DateTime.UtcNow.ToLocalTime(),
                        EmailRequestParam = JsonDocument.Parse(JsonConvert.SerializeObject(requestVM)),
                        EmailResponseParam = JsonDocument.Parse("{}"),
                        Status = Status.Failed
                    };
                    await _emailVerificationRepository.AddAsync(emailVerification);
                    throw new Exception(Messages.Failed, ex);
                }
                emailVerification = new()
                {
                    UserId = id,
                    CreatedAt = DateTime.UtcNow.ToLocalTime(),
                    EmailRequestParam = JsonDocument.Parse(JsonConvert.SerializeObject(requestVM)),
                    EmailResponseParam = JsonDocument.Parse(JsonConvert.SerializeObject(responseVM.Result)),
                    Status = Status.Success,
                };
                await _emailVerificationRepository.AddAsync(emailVerification);
                return responseVM.Result ?? new EmailVerificationResponseVM();
            }
            else
            {
                throw new Exception("email is null or empty!");
            }
        }

        public async Task<EmailVerificationDetailVM> GetEmailDetailedHistory(string id)
        {
            int originalId = _encryptDecryptServices.DecryptId(id);
            EmailVerification emailVerification = await _emailVerificationRepository.GetAsync(originalId);

            //Deserialization
            EmailVerificationResponseVM responseVM = JsonConvert.DeserializeObject<EmailVerificationResponseVM>(emailVerification.EmailResponseParam.RootElement.GetRawText()) ?? new EmailVerificationResponseVM();
            EmailVerificationRequestVM requestVM = emailVerification?.EmailRequestParam.RootElement.Deserialize<EmailVerificationRequestVM>() ?? new EmailVerificationRequestVM();

            return new EmailVerificationDetailVM
            {
                RequestVM = requestVM,
                ResponseVM = responseVM,
                CreatedAt = emailVerification!.CreatedAt
            };
        }
    }
}
