using MicroservicesUser.Models.DTO;

namespace MicroservicesUser.Models.ViewModels.Dashboard
{
    public class AdminEmailVerificationDashboardVM : PaginationDTO
    {
        public int TotalEmailVerifications { get; set; }
        public double AverageFraudScore { get; set; }
        public int ValidEmails { get; set; }
        public double SuccessRate { get; set; }
        public List<DashboardUserVM>? DashboardUsers { get; set; }
        public List<AdminEmailVerificationHistoryListVM>? EmailVerificationHistoryList { get; set; }
        public List<EmailVerificationDateTimeStatesVM>? EmailVerificationDateTimeStates { get; set; }
    }

    public class AdminEmailVerificationHistoryListVM
    {
        public int? Id { get; set; }
        public string? Username { get; set; }
        public string? VerifiedEmail { get; set; }
        public int? FraudScore { get; set; }
        public string? UserStatus { get; set; }
        public string? ScannedStatus { get; set; }
        public bool Valid { get; set; }
    }
    public class EmailVerificationDateTimeStatesVM
    {
        public int EmailVerificationCount { get; set; }
        public string? CreatedAt { get; set; }
    }
}
