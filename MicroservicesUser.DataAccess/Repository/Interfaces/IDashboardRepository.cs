using MicroservicesUser.Models.DTO;

namespace MicroservicesUser.DataAccess.Repository.Interfaces
{
    public interface IDashboardRepository
    {
        public Task<DashboardDTO> GetDashboardAsync(int userId);
    }
}
