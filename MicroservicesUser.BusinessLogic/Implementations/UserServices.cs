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
        private readonly IEncryptDecryptServices _encryptDecryptServices;
        public UserServices(IUserRepository userRepository, IMapper mapper, IEncryptDecryptServices encryptDecryptServices)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _encryptDecryptServices = encryptDecryptServices;
        }

        public async Task<UserListVM> GetUserList(PaginationDTO paginationDTO)
        {
            if (string.IsNullOrEmpty(paginationDTO.ColumnNameForSorting))
            {
                paginationDTO.ColumnNameForSorting = "CreatedAt";
                paginationDTO.OrderOfSorting = "desc";
            }
            (List<User> users, int totalCount) = await _userRepository.GetListAsync(paginationDTO);
            List<UserVM> userVMs = _mapper.Map<List<UserVM>>(users);
            for (int i = 0; i < userVMs.Count; i++)
            {
                userVMs[i].Id = _encryptDecryptServices.EncryptId(users[i].Id);
            }
            UserListVM userListVM = _mapper.Map<UserListVM>(paginationDTO);
            userListVM.Users = userVMs;
            userListVM.TotalItems = totalCount;
            return userListVM;
        }

        public async Task<UserVM> BlockUnblockUser(string id)
        {
            int originalId = _encryptDecryptServices.DecryptId(id);
            User? user = await _userRepository.GetByIdAsync(originalId);
            user!.IsBlocked = !user.IsBlocked;
            user.UpdatedAt = DateTime.UtcNow.ToLocalTime();
            await _userRepository.UpdateAsync(user);
            UserVM userVM = _mapper.Map<UserVM>(user);
            userVM.Id = id;
            return userVM;
        }

        public async Task<UserVM> DeleteUser(string id)
        {
            int originalId = _encryptDecryptServices.DecryptId(id);
            User? user = await _userRepository.GetByIdAsync(originalId);
            user!.IsDeleted = true;
            user.IsBlocked = true;
            user.UpdatedAt = DateTime.UtcNow.ToLocalTime();
            await _userRepository.UpdateAsync(user);
            UserVM userVM = _mapper.Map<UserVM>(user);
            return userVM;
        }
    }

}
