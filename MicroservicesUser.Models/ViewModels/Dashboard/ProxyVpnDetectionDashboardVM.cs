using MicroservicesUser.Models.DTO;

namespace MicroservicesUser.Models.ViewModels.Dashboard
{
    public class ProxyVpnDetectionDashboardVM : PaginationDTO
    {
        public int TotalIPVerifications { get; set; }
        public double AverageFraudScore { get; set; }
        public int HighRiskIPs { get; set; }
        public double VPNProxyTorUsage { get; set; }
        public List<DashboardUserVM>? DashboardUsers { get; set; }
        public List<ProxyVpnDetectionHistoryListVM>? ProxyVpnDetectionHistoryList { get; set; }
    }

    public class ProxyVpnDetectionHistoryListVM
    {
        public int? Id { get; set; }
        public string? Email { get; set; }
        public string? IpAddress { get; set; }
        public string? RiskStatus { get; set; }
        public string VPNProxyTor { get; set; } = "Normal";

    }
}
