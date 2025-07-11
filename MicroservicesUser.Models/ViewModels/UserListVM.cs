using MicroservicesUser.Models.DTO;

namespace MicroservicesUser.Models.ViewModels
{
    public class UserListVM : PaginationDTO
    {
        public List<UserVM>? Users { get; set; }
    }
    public class UserVM
    {
        public int Id { get; set; }
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? MobileNumber { get; set; }
        public string? ProfilePhotoGeneratedName { get; set; }
        public string? Address { get; set; }
        public string? DateTime { get; set; }
    }
}