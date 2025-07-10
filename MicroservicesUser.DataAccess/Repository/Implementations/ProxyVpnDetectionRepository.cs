using MicroservicesUser.DataAccess.Data;
using MicroservicesUser.DataAccess.Repository.Interfaces;
using MicroservicesUser.Models.DTO;
using MicroservicesUser.Models.Models;
using Microsoft.EntityFrameworkCore;

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

        public async Task<ProxyVpnDetection?> GetAsync(int id)
        {
            return await _context.ProxyVpnDetections.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<ProxyVpnDetectionDashboardDTO> GetListAsync(ProxyVpnDetectionHistoryRequestDTO requestDto)
        {
            List<ProxyVpnDetection> data = await _context.ProxyVpnDetections
            .Include(u => u.User)
            .Where(u => requestDto.UserIds!.Count == 0 || requestDto.UserIds == null || requestDto.UserIds.Contains(u.UserId))
            .ToListAsync();

            if (!string.IsNullOrEmpty(requestDto.PaginationDTO!.SearchQuery))
            {
                data = data.Where(x => x.User!.Email.ToLower().Contains(requestDto.PaginationDTO.SearchQuery.ToLower())).ToList();
            }
            List<ProxyVpnDetection> latestEntries = data
                .GroupBy(u =>
                {
                    var json = u.ProxyVpnRequestParam;
                    var ip = json.RootElement.TryGetProperty("IpAddress", out var ipProp) ? ipProp.GetString() : "unknown";
                    return new { u.UserId, IpAddress = ip };
                })
                .Select(g => g.OrderBy(u => u.CreatedAt).First())
                .ToList();


            List<ProxyVpnDetectionHistoryListDTO> filteredEntries = latestEntries.Select(x =>
            {
                int fraudScore = x.ProxyVpnResponseParam.RootElement.TryGetProperty("fraudScore", out var scoreElement)
                    ? scoreElement.GetInt16()
                    : 0;

                string? riskLevel = fraudScore > 75 ? "High" :
                                    fraudScore >= 25 ? "Medium" : "Low";

                bool isTor = x.ProxyVpnResponseParam.RootElement.TryGetProperty("tor", out var torElement) && torElement.GetBoolean();
                bool isVpn = x.ProxyVpnResponseParam.RootElement.TryGetProperty("vpn", out var vpnElement) && vpnElement.GetBoolean();
                bool isProxy = x.ProxyVpnResponseParam.RootElement.TryGetProperty("proxy", out var proxyElement) && proxyElement.GetBoolean();

                string? connectionType = isTor ? "TOR" :
                                         isVpn ? "VPN" :
                                         isProxy ? "Proxy" :
                                         "Normal";

                return new ProxyVpnDetectionHistoryListDTO
                {
                    Id = x.Id,
                    Email = x.User!.Email,
                    IpAddress = x.ProxyVpnRequestParam.RootElement.GetProperty("IpAddress").ToString(),
                    RiskStatus = riskLevel,
                    VPNProxyTor = connectionType,
                };
            })
            .Where(dto =>
                (string.IsNullOrEmpty(requestDto.RiskStatus) || dto.RiskStatus!.ToLower() == requestDto.RiskStatus.ToLower()) &&
                (string.IsNullOrEmpty(requestDto.ConnectionType) || dto.VPNProxyTor.ToLower() == requestDto.ConnectionType.ToLower()))
            .ToList();

            List<ProxyVpnDetectionHistoryListDTO> paginatedEntries = filteredEntries
                .Skip((requestDto.PaginationDTO!.CurrentPage - 1) * requestDto.PaginationDTO.PageSize)
                .Take(requestDto.PaginationDTO.PageSize)
                .ToList();

            return new ProxyVpnDetectionDashboardDTO
            {
                ProxyVpnDetectionHistoryList = paginatedEntries,
                CurrentPage = requestDto.PaginationDTO.CurrentPage,
                PageSize = requestDto.PaginationDTO.PageSize,
                TotalItems = filteredEntries.Count
            };

        }
    }
}