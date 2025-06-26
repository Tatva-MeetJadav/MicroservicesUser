
namespace MicroservicesUser.Models.DTO
{
    public class DashboardDTO
    {
        public int TotalCount { get; set; }
        public int TodayCount { get; set; }
        public double SuccessRate { get; set; }
        public int ValidCount { get; set; }
        public List<string>? Labels { get; set; }
        public List<int>? Scans { get; set; }
        public List<EmailVerificationDTO>? EmailVerificationList { get; set; }
    }
}
