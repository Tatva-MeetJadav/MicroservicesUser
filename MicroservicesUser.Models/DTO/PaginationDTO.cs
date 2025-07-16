namespace MicroservicesUser.Models.DTO
{
    public class PaginationDTO
    {
        public string SearchQuery { get; set; } = string.Empty;
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 5;
        public int TotalItems { get; set; }
        public string ColumnNameForSorting { get; set; } = string.Empty;
        public string OrderOfSorting { get; set; } = "asc";
        public string ColumnNameForFilter { get; set; } = string.Empty;
        public string FilterValue { get; set; } = "False";
    }
}