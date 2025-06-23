namespace MicroservicesUser.Models.ViewModels.History
{
    public class EmailVerificationHistoryVM
    {
        public string Email { get; set; } = string.Empty;
        public bool Valid { get; set; }
        public DateTime? VerifiedAt { get; set; }
        public string Deliverability { get; set; } = string.Empty;
        public int OverAllScore { get; set; }
    }

    public class EmailVerificationListHistoryVM : PaginationVM
    {
        public List<EmailVerificationHistoryVM>? EmailVerificationHistoryListVM { get; set; }

    }
}