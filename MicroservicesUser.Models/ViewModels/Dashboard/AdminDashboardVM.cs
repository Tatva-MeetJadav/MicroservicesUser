namespace MicroservicesUser.Models.ViewModels.Dashboard
{
    public class AdminDashboardVM
    {
        public int TotalUserRegistrations { get; set; }
        public int TotalEmailVerifications { get; set; }
        public int TotalIPVerifications { get; set; }
        public int TotalHelpDeskRequest { get; set; }
        public List<UserRegistrationsChartVM>? UserRegistrationsChart { get; set; }
        public ServiceUsageChartVM? ServiceUsageChart { get; set; }
    }
    public class UserRegistrationsChartVM
    {
        public int UserCount { get; set; }
        public string CreatedAt { get; set; } = string.Empty;
    }

    public class ServiceUsageChartVM
    {
        public int RequestCount { get; set; }
        public int DailyLimit { get; set; }
    }
}
