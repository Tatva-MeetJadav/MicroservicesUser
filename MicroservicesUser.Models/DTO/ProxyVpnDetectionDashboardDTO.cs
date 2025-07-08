namespace MicroservicesUser.Models.DTO
{
    public class ProxyVpnDetectionDashboardDTO
    {
        public int TotalIPVerifications { get; set; }
        public double AverageFraudScore { get; set; }
        public int HighRiskIPs { get; set; }
        public double VPNProxyTorUsage { get; set; }
        public List<DashboardUserDTO>? DashboardUsers { get; set; }

    }
}
