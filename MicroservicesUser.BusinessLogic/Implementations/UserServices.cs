using AutoMapper;
using MicroservicesUser.BusinessLogic.Interfaces;
using MicroservicesUser.DataAccess.Repository.Interfaces;
using MicroservicesUser.Models.DTO;
using MicroservicesUser.Models.Models;
using MicroservicesUser.Models.ViewModels;


namespace MicroservicesUser.BusinessLogic.Implementations
{
    public class UserServices : IUserServices
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public UserServices(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserListVM> GetUserList(PaginationDTO paginationDTO)
        {
            (List<User> users, int totalCount) = await _userRepository.GetListAsync(paginationDTO);
            List<UserVM> userVMs = _mapper.Map<List<UserVM>>(users);
            UserListVM userListVM = _mapper.Map<UserListVM>(paginationDTO);
            userListVM.Users = userVMs;
            userListVM.TotalItems = totalCount;
            return userListVM;
        }
    }

}
