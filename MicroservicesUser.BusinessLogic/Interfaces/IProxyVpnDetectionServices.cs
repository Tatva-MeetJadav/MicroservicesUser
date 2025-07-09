using MicroservicesUser.Models.DTO;
using MicroservicesUser.Models.ViewModels.Dashboard;

namespace MicroservicesUser.BusinessLogic.Interfaces
{
    public interface IProxyVpnDetectionServices
    {
        Task<ProxyVpnDetectionDashboardVM> GetProxyVpnDetectionDashboard(List<int>? userIds);
        Task<ProxyVpnDetectionDashboardVM> GetProxyVpnDetectionHistoryList(ProxyVpnDetectionHistoryRequestDTO requestDto);
    }
}
