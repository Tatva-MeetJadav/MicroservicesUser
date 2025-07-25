
namespace MicroservicesUser.Models.DTO
{
    public class AdminDashboardDTO
    {
        public int TotalUserRegistrations { get; set; }
        public int TotalEmailVerifications { get; set; }
        public int TotalIPVerifications { get; set; }
        public int TotalHelpDeskRequest { get; set; }
        public List<UserRegistrationsChartDTO>? UserRegistrationsChart { get; set; }
        public ServiceUsageChartDTO? ServiceUsageChart { get; set; }
    }

    public class UserRegistrationsChartDTO
    {
        public int UserCount { get; set; }
        public string CreatedAt { get; set; } = string.Empty;
    }

    public class ServiceUsageChartDTO
    {
        public int RequestCount { get; set; }
        public int DailyLimit { get; set; }
    }
}