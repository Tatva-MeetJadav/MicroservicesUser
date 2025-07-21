namespace MicroservicesUser.Models.DTO
{
    public class AdminEmailVerificationDashboardDTO : PaginationDTO
    {
        public int TotalEmailVerifications { get; set; }
        public double AverageFraudScore { get; set; }
        public int ValidEmails { get; set; }
        public double SuccessRate { get; set; }
        public List<DashboardUserDTO>? DashboardUsers { get; set; }
        public List<AdminEmailVerificationHistoryListDTO>? EmailVerificationHistoryList { get; set; }
        public List<EmailVerificationDateTimeStatesDTO>? EmailVerificationDateTimeStates { get; set; }

    }

    public class AdminEmailVerificationHistoryListDTO
    {
        public int? Id { get; set; }
        public string? Username { get; set; }
        public string? VerifiedEmail { get; set; }
        public int? FraudScore { get; set; }
        public string? UserStatus { get; set; }
        public string? ScannedStatus { get; set; }
        public string? Valid { get; set; }
    }

    public class EmailVerificationDateTimeStatesDTO
    {
        public int EmailVerificationCount { get; set; }
        public string? CreatedAt { get; set; }
    }
}
