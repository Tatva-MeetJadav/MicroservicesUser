using MicroservicesUser.Models.DTO;
using MicroservicesUser.Models.ViewModels;

namespace MicroservicesUser.BusinessLogic.Interfaces
{
    public interface IUserServices
    {
        Task<UserListVM> GetUserList(PaginationDTO paginationDTO);
        Task<UserVM> BlockUnblockUser(string id);
        Task<UserVM> DeleteUser(string id);
    }
}
