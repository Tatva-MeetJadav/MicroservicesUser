namespace MicroservicesUser.Models.DTO
{
    public class ProxyVpnDetectionHistoryRequestDTO
    {
        public List<int>? UserIds { get; set; }
        public PaginationDTO? PaginationDTO { get; set; }
        public string? ConnectionType { get; set; }
        public string? RiskStatus { get; set; }
    }
}
