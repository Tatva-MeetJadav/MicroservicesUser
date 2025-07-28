using System.Text.Json;
using AutoMapper;
using MicroservicesUser.BusinessLogic.Interfaces;
using MicroservicesUser.Common.ResourcesFiles;
using MicroservicesUser.DataAccess.Repository.Interfaces;
using MicroservicesUser.Models.DTO;
using MicroservicesUser.Models.Enums;
using MicroservicesUser.Models.Models;
using MicroservicesUser.Models.ViewModels;
using MicroservicesUser.Models.ViewModels.Dashboard;
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
        private readonly IDashboardRepository _dashboardRepository;
        private readonly IMapper _mapper;

        public EmailVerificationServices(IGenericAPIClientServices apiClient, IConfiguration configuration, IEmailVerificationRepository emailVerificationRepository, IJwtServices jwtServices, IEncryptDecryptServices encryptDecryptServices, IDashboardRepository dashboardRepository, IMapper mapper)
        {
            _apiClient = apiClient;
            _configuration = configuration;
            _emailVerificationRepository = emailVerificationRepository;
            _jwtServices = jwtServices;
            _encryptDecryptServices = encryptDecryptServices;
            _dashboardRepository = dashboardRepository;
            _mapper = mapper;
        }

        public async Task<EmailVerificationListHistoryVM> GetEmailVerificationListHistory(PaginationDTO PaginationDTO, string token)
        {
            int id = _jwtServices.GetUserId(token);
            byte[] key = Convert.FromBase64String(_configuration["EncryptId:Key"] ?? string.Empty);
            byte[] iv = Convert.FromBase64String(_configuration["EncryptId:IV"] ?? string.Empty);
            if (string.IsNullOrEmpty(PaginationDTO.ColumnNameForSorting))
            {
                PaginationDTO.OrderOfSorting = "desc";
                PaginationDTO.ColumnNameForSorting = "CreatedAt";
            }
            (List<EmailVerification> emailVerifications, int count) = await _emailVerificationRepository.GetListByUserId(id, PaginationDTO);
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
                PageSize = PaginationDTO.PageSize,
                CurrentPage = PaginationDTO.CurrentPage,
                ColumnNameForSorting = PaginationDTO.ColumnNameForSorting,
                OrderOfSorting = PaginationDTO.OrderOfSorting,
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

        public async Task<AdminEmailVerificationDashboardVM> GetAdminEmailVerificationDashboard(List<int>? userIds)
        {
            AdminEmailVerificationDashboardDTO dashboardDTO = await _dashboardRepository.GetAdminEmailVerificationDashboardAsync(userIds);
            AdminEmailVerificationDashboardVM dashboardVM = _mapper.Map<AdminEmailVerificationDashboardVM>(dashboardDTO);
            return dashboardVM;
        }

        public async Task<AdminEmailVerificationDashboardVM> GetAdminEmailVerificationHistoryList(AdminEmailVerificationHistoryRequestDTO requestDTO)
        {
            AdminEmailVerificationDashboardDTO dashboardDTO = await _emailVerificationRepository.GetEmailVerificationHistoryList(requestDTO);
            AdminEmailVerificationDashboardVM dashboardVM = _mapper.Map<AdminEmailVerificationDashboardVM>(dashboardDTO);
            return dashboardVM;
        }

        public async Task<List<EmailVerificationDateTimeStatesVM>> GetAdminEmailVerificationChart(List<int>? userIds, string range)
        {
            List<EmailVerificationDateTimeStatesDTO> dto = await _dashboardRepository.GetAdminEmailVerificationChart(userIds, range);
            List<EmailVerificationDateTimeStatesVM> resultVm = _mapper.Map<List<EmailVerificationDateTimeStatesVM>>(dto);
            return resultVm;
        }

        public async Task<AdminEmailVerificationDetailVM> GetAdminEmailVerificationDetail(int id)
        {
            EmailVerification emailVerification = await _emailVerificationRepository.GetAsync(id);
            AdminEmailVerificationDetailVM result = JsonConvert.DeserializeObject<AdminEmailVerificationDetailVM>(emailVerification.EmailResponseParam.RootElement.GetRawText()) ?? new AdminEmailVerificationDetailVM();
            return result;
        }
    }
}
