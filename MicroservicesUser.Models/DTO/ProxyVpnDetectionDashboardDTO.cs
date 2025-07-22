namespace MicroservicesUser.Models.DTO
{
    public class ProxyVpnDetectionDashboardDTO : PaginationDTO
    {
        public int TotalIPVerifications { get; set; }
        public double AverageFraudScore { get; set; }
        public int HighRiskIPs { get; set; }
        public double VPNProxyTorUsage { get; set; }
        public List<DashboardUserDTO>? DashboardUsers { get; set; }
        public List<ProxyVpnDetectionHistoryListDTO>? ProxyVpnDetectionHistoryList { get; set; }
        public List<ContinentIPStatsDTO>? ContinentIPStats { get; set; }

    }

    public class ProxyVpnDetectionHistoryListDTO
    {
        public int? Id { get; set; }
        public string? Username { get; set; }
        public string? Status { get; set; }
        public string? IpAddress { get; set; }
        public string? RiskStatus { get; set; }
        public string VPNProxyTor { get; set; } = "Normal";

    }
    public class ContinentIPStatsDTO
    {
        public string ContinentName { get; set; } = "Unknown";
        public double Percentage { get; set; }
    }

}
