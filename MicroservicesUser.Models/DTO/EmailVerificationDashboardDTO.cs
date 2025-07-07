namespace MicroservicesUser.Models.DTO
{
    public class EmailVerificationDashboardDTO
    {
        public int TotalCount { get; set; }
        public int TodayCount { get; set; }
        public double SuccessRate { get; set; }
        public int ValidCount { get; set; }
        public ChartDTO? ChartData { get; set; }
        public List<EmailVerificationDTO>? EmailVerificationList { get; set; }
    }

}
