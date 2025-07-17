namespace MicroservicesUser.Models.DTO
{
    public class AdminEmailVerificationHistoryRequestDTO
    {
        public List<int>? UserIds { get; set; }
        public PaginationDTO? PaginationDTO { get; set; }
        public string? ScannedStatus { get; set; }
        public bool? Valid { get; set; }
    }
}
