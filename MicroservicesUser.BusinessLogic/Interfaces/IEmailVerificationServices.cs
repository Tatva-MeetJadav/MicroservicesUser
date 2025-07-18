using MicroservicesUser.Models.DTO;
using MicroservicesUser.Models.ViewModels.Dashboard;
using MicroservicesUser.Models.ViewModels.EmailVerification;
using MicroservicesUser.Models.ViewModels.History;

namespace MicroservicesUser.BusinessLogic.Interfaces
{
    public interface IEmailVerificationServices
    {
        Task<EmailVerificationResponseVM> VerifyEmail(EmailVerificationRequestVM requestVM, string token);
        Task<EmailVerificationListHistoryVM> GetEmailVerificationListHistory(PaginationDTO paginationDTO, string token);
        Task<EmailVerificationDetailVM> GetEmailDetailedHistory(string id);
        Task<AdminEmailVerificationDashboardVM> GetAdminEmailVerificationDashboard(List<int> userIds);
        Task<AdminEmailVerificationDashboardVM> GetAdminEmailVerificationHistoryList(AdminEmailVerificationHistoryRequestDTO requestDTO);
        Task<List<EmailVerificationDateTimeStatesVM>> GetAdminEmailVerificationChart(List<int> userIds, string range);
    }
}
