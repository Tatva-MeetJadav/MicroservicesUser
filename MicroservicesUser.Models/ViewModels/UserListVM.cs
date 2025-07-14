using MicroservicesUser.Models.DTO;

namespace MicroservicesUser.Models.ViewModels
{
    public class UserListVM : PaginationDTO
    {
        public List<UserVM>? Users { get; set; }
    }
    public class UserVM
    {
        public string? Id { get; set; }
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? MobileNumber { get; set; }
        public string? ProfilePhotoGeneratedName { get; set; }
        public string? Address { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsBlocked { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
    }
}