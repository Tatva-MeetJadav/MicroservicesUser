using MicroservicesUser.Models.DTO;

namespace MicroservicesUser.DataAccess.Repository.Interfaces
{
    public interface IDashboardRepository
    {
        Task<EmailVerificationDashboardDTO> GetEmailVerificationDashboardAsync(int userId);
        Task<ChartDTO> GetEmailVerificationChartByRangeAsync(int userId, string range);
        Task<ProxyVpnDetectionDashboardDTO> GetProxyVpnDetectionDashboard(List<int>? userId);
        Task<AdminEmailVerificationDashboardDTO> GetAdminEmailVerificationDashboardAsync(List<int>? userIds);
        Task<List<EmailVerificationDateTimeStatesDTO>> GetAdminEmailVerificationChart(List<int>? userIds, string range);

    }
}
