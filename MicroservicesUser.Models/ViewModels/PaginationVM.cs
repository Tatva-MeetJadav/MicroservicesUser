namespace MicroservicesUser.Models.ViewModels
{
    public class PaginationVM
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int CurrentPageItems { get; set; }
    }
}