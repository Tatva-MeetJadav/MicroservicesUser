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
        private readonly IEmailVerificationRepository _emailVerificationRepository;
        private readonly IDashboardRepository _dashboardRepository;
        public DashboardServices(IUserRepository userRepository, IJwtServices jwtServices, IMapper mapper, IWebHostEnvironment webHostEnvironment, IConfiguration configuration, IEncryptDecryptServices encryptDecryptServices, IEmailVerificationRepository emailVerificationRepository, IDashboardRepository dashboardRepository)
        {
            _userRepository = userRepository;
            _jwtServices = jwtServices;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
            _encryptDecryptServices = encryptDecryptServices;
            _emailVerificationRepository = emailVerificationRepository;
            _dashboardRepository = dashboardRepository;
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
            DashboardDTO dashboardDTO = await _dashboardRepository.GetDashboardAsync(id);
            EmailVerificationDashboardVM dashboardVM = _mapper.Map<EmailVerificationDashboardVM>(dashboardDTO);
            return dashboardVM;
        }

        public async Task<string> GetProfilePhoto(string token)
        {
            int id = _jwtServices.GetUserId(token);
            User? user = await _userRepository.GetByIdAsync(id);
            return user?.ProfilePhotoGeneratedName ?? string.Empty;
        }
    }
}