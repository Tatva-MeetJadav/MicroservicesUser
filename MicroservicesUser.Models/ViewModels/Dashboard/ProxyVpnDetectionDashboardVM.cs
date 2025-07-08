namespace MicroservicesUser.Models.ViewModels.Dashboard
{
    public class ProxyVpnDetectionDashboardVM
    {
        public int TotalIPVerifications { get; set; }
        public double AverageFraudScore { get; set; }
        public int HighRiskIPs { get; set; }
        public double VPNProxyTorUsage { get; set; }
        public List<DashboardUserVM>? DashboardUsers { get; set; }

    }
}
