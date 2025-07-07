using MicroservicesUser.Models.Models;

namespace MicroservicesUser.DataAccess.Repository.Interfaces
{
    public interface IProxyVpnDetectionRepository
    {
        Task AddAsync(ProxyVpnDetection proxyVpnDetection);
    }
}