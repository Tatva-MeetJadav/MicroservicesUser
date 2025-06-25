using MicroservicesUser.Models.ViewModels.History;

namespace MicroservicesUser.Models.ViewModels.Dashboard
{
    public class EmailVerificationDashboardVM
    {
        public int TotalVerificationCount { get; set; }
        public double SuccessRate { get; set; }
        public int TodayCount { get; set; }
        public int ValidCount { get; set; }
        public List<EmailVerificationHistoryVM>? RecentVerificationList { get; set; }
    }
}
