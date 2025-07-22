namespace MicroservicesUser.Models.DTO
{
    public class HelpAndSupportRequestDTO
    {
        public PaginationDTO? PaginationDTO { get; set; } = new();
        public string? CategoryType { get; set; }
        public DateOnly FromDate { get; set; } = DateOnly.FromDateTime(DateTime.MinValue);
        public DateOnly ToDate { get; set; } = DateOnly.FromDateTime(DateTime.MaxValue);
    }
}
