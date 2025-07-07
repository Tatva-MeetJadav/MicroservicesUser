using MicroservicesUser.Models.DTO;

namespace MicroservicesUser.DataAccess.Repository.Interfaces
{
    public interface IDashboardRepository
    {
        Task<EmailVerificationDashboardDTO> GetEmailVerificationDashboardAsync(int userId);
        Task<ChartDTO> GetEmailVerificationChartByRangeAsync(int userId, string range);
        Task<AdminDashboardDTO> GetAdminDashboardAsync();
    }
}
