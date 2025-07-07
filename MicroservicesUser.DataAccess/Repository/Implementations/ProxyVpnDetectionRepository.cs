using MicroservicesUser.DataAccess.Data;
using MicroservicesUser.DataAccess.Repository.Interfaces;
using MicroservicesUser.Models.Models;

namespace MicroservicesUser.DataAccess.Repository.Implementations
{
    public class ProxyVpnDetectionRepository : IProxyVpnDetectionRepository
    {
        private readonly MicroservicesUserDbContext _context;

        public ProxyVpnDetectionRepository(MicroservicesUserDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(ProxyVpnDetection proxyVpnDetection)
        {
            await _context.ProxyVpnDetections.AddAsync(proxyVpnDetection);
            await _context.SaveChangesAsync();
        }
    }
}