namespace MicroservicesUser.Models.DTO
{
    public class PaginationDTO
    {
        public string SearchQuery { get; set; } = string.Empty;
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public string ColumnNameForSorting { get; set; } = string.Empty;
        public string OrderOfSorting { get; set; } = string.Empty;
        public string ColumnNameForFilter { get; set; } = string.Empty;
        public bool FilterValue { get; set; } = false;
    }
}