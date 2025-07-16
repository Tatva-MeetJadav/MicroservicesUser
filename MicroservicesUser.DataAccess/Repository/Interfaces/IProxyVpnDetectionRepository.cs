using MicroservicesUser.Models.DTO;
using MicroservicesUser.Models.Models;

namespace MicroservicesUser.DataAccess.Repository.Interfaces
{
    public interface IProxyVpnDetectionRepository
    {
        Task AddAsync(ProxyVpnDetection proxyVpnDetection);
        Task<ProxyVpnDetectionDashboardDTO> GetListAsync(ProxyVpnDetectionHistoryRequestDTO requestDto);
        Task<ProxyVpnDetection?> GetAsync(int id);
    }
}