using AutoMapper;
using MicroservicesUser.BusinessLogic.Interfaces;
using MicroservicesUser.BusinessLogic.Utilities;
using MicroservicesUser.Common.ResourcesFiles;
using MicroservicesUser.DataAccess.Repository.Interfaces;
using MicroservicesUser.Models.DTO;
using MicroservicesUser.Models.Models;
using MicroservicesUser.Models.ViewModels;
using MicroservicesUser.Models.ViewModels.Dashboard;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace MicroservicesUser.BusinessLogic.Implementations
{
    public class DashboardServices : IDashboardServices
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtServices _jwtServices;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;
        private readonly IEncryptDecryptServices _encryptDecryptServices;
        private readonly IDashboardRepository _dashboardRepository;

        private readonly IAdminRepository _adminRepository;
        public DashboardServices(IUserRepository userRepository, IJwtServices jwtServices, IMapper mapper, IWebHostEnvironment webHostEnvironment, IConfiguration configuration, IEncryptDecryptServices encryptDecryptServices, IDashboardRepository dashboardRepository, IAdminRepository adminRepository)
        {
            _userRepository = userRepository;
            _jwtServices = jwtServices;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
            _encryptDecryptServices = encryptDecryptServices;
            _dashboardRepository = dashboardRepository;
            _adminRepository = adminRepository;
        }

        public async Task<ProfileVM> GetUserProfile(string token)
        {
            int id = _jwtServices.GetUserId(token);
            User? user = await _userRepository.GetByIdAsync(id);
            ProfileVM profileVM = _mapper.Map<ProfileVM>(user);
            profileVM.ProfilePhotoName = UploadFile.GetOriginalPhotoName(user!.ProfilePhotoGeneratedName!) ?? string.Empty;
            return profileVM;
        }

        public async Task<string> EditUserProfile(ProfileVM profileVM, IFormFile file)
        {
            User? user = await _userRepository.GetByIdAsync(profileVM.Id);
            if (user != null)
            {
                _mapper.Map(profileVM, user);

                if (file != null)
                {
                    string imageUrl = await UploadFile.UploadPhotoAsync(file, _webHostEnvironment.WebRootPath, _configuration["PhotosPath:ProfilePhoto"] ?? string.Empty) ?? string.Empty;
                    user.ProfilePhotoGeneratedName = imageUrl;
                }
                await _userRepository.UpdateAsync(user);
                return Messages.SuccessMessage;
            }
            else
            {
                return string.Empty;
            }
        }

        public async Task<string> ChangePassword(ChangePasswordVM changePasswordVM, string token)
        {
            int id = _jwtServices.GetUserId(token);
            User? user = await _userRepository.GetByIdAsync(id);
            if (user != null)
            {
                if (_encryptDecryptServices.VerifyPassword(changePasswordVM.CurrentPassword, user.PasswordHash))
                {
                    string hashedPassword = _encryptDecryptServices.EncryptPassword(changePasswordVM.NewPassword);
                    user.PasswordHash = hashedPassword;
                    await _userRepository.UpdateAsync(user);
                    return Messages.SuccessMessage;
                }
                else
                {
                    return Messages.WrongPassword;
                }
            }
            else
            {
                return string.Empty;
            }
        }

        public async Task<EmailVerificationDashboardVM> GetEmailVerificationDashboard(string token)
        {
            int id = _jwtServices.GetUserId(token);
            EmailVerificationDashboardDTO dashboardDTO = await _dashboardRepository.GetEmailVerificationDashboardAsync(id);
            EmailVerificationDashboardVM dashboardVM = _mapper.Map<EmailVerificationDashboardVM>(dashboardDTO);
            return dashboardVM;
        }

        public async Task<string> GetProfilePhoto(string token)
        {
            int id = _jwtServices.GetUserId(token);
            string role = _jwtServices.GetRole(token);
            if (role == Messages.UserRole)
            {
                User? user = await _userRepository.GetByIdAsync(id);
                return user?.ProfilePhotoGeneratedName ?? string.Empty;
            }
            else
            {
                Admin? admin = await _adminRepository.GetByIdAsync(id);
                return admin?.ProfilePhotoGeneratedName ?? string.Empty;
            }
        }

        public async Task<EmailVerificationChart> GetChartData(string token, string range)
        {
            int id = _jwtServices.GetUserId(token);
            ChartDTO chartDTO = await _dashboardRepository.GetEmailVerificationChartByRangeAsync(id, range);
            EmailVerificationChart chartVM = _mapper.Map<EmailVerificationChart>(chartDTO);
            return chartVM;
        }

        public async Task<ProfileVM> GetAdminProfile(string token)
        {
            int id = _jwtServices.GetUserId(token);
            Admin? admin = await _adminRepository.GetByIdAsync(id);
            ProfileVM profileVM = _mapper.Map<ProfileVM>(admin);
            return profileVM;
        }

        public async Task<string> EditAdminProfile(ProfileVM profileVM, IFormFile file)
        {
            Admin? admin = await _adminRepository.GetByIdAsync(profileVM.Id);
            if (admin != null)
            {
                _mapper.Map(profileVM, admin);
                if (file != null)
                {
                    string imageUrl = await UploadFile.UploadPhotoAsync(file, _webHostEnvironment.WebRootPath, _configuration["PhotosPath:ProfilePhoto"] ?? string.Empty) ?? string.Empty;
                    admin.ProfilePhotoGeneratedName = imageUrl;
                }
                await _adminRepository.UpdateAsync(admin);
                return Messages.SuccessMessage;
            }
            else
            {
                return string.Empty;
            }
        }

        public async Task<string> AdminChangePassword(ChangePasswordVM changePasswordVM, string token)
        {
            int id = _jwtServices.GetUserId(token);
            Admin? admin = await _adminRepository.GetByIdAsync(id);
            if (admin != null)
            {
                if (_encryptDecryptServices.VerifyPassword(changePasswordVM.CurrentPassword, admin.PasswordHash))
                {
                    string hashedPassword = _encryptDecryptServices.EncryptPassword(changePasswordVM.NewPassword);
                    admin.PasswordHash = hashedPassword;
                    await _adminRepository.UpdateAsync(admin);
                    return Messages.SuccessMessage;
                }
                else
                {
                    return Messages.WrongPassword;
                }
            }
            else
            {
                return string.Empty;
            }
        }

        // public async Task<string> GetAdminDashboard()
        // {

        // }
    }
}