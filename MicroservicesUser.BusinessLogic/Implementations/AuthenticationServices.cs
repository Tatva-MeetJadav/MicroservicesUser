using AutoMapper;
using MicroservicesUser.BusinessLogic.Interfaces;
using MicroservicesUser.DataAccess.Repository.Interfaces;
using MicroservicesUser.Models.Models;
using MicroservicesUser.Models.ViewModels;
using Microservices.BusinessLogic.Utilities;
using Microservices.BusinessLogic.Interfaces;
using MicroservicesUser.Common.ResourcesFiles;
using MicroservicesUser.BusinessLogic.ServerStorage.Interfaces;
using Microsoft.Extensions.Configuration;

namespace MicroservicesUser.BusinessLogic.Implementations
{
    public class AuthenticationServices : IAuthenticationServices
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IJwtServices _jwtServices;
        private readonly IEmailServices _emailServices;
        private readonly ITokenStore _tokenStore;
        private readonly IConfiguration _configuration;
        private readonly IEncryptDecryptServices _encryptDecryptServices;
        private readonly IAdminRepository _adminRepository;
        public AuthenticationServices(IMapper mapper, IUserRepository userRepository, IJwtServices jwtServices, IEmailServices emailServices, ITokenStore tokenStore, IConfiguration configuration, IEncryptDecryptServices encryptDecryptServices, IAdminRepository adminRepository)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _jwtServices = jwtServices;
            _emailServices = emailServices;
            _tokenStore = tokenStore;
            _configuration = configuration;
            _encryptDecryptServices = encryptDecryptServices;
            _adminRepository = adminRepository;
        }
        public async Task<string> RegisterUser(RegisterVM registerVM)
        {
            User? ifAlreadyExist = await _userRepository.GetByEmailAsync(registerVM.Email);
            if (ifAlreadyExist == null)
            {
                string passwordHash = _encryptDecryptServices.EncryptPassword(registerVM.Password);
                registerVM.Password = passwordHash;
                User user = _mapper.Map<User>(registerVM);
                await _userRepository.AddAsync(user);
                return Messages.SuccessMessage;
            }
            else
            {
                return Messages.DuplicateValue;
            }
        }

        public async Task<string> LoginUser(LoginVM loginVM)
        {
            if (loginVM.IsAdmin == false)
            {
                User? user = await _userRepository.GetByEmailAsync(loginVM.Email);
                if (user != null)
                {
                    string dbPassword = user.PasswordHash;
                    if (_encryptDecryptServices.VerifyPassword(loginVM.Password, dbPassword))
                    {
                        string token = _jwtServices.GenerateJwtToken(user.Id);
                        double hours = Convert.ToDouble(_configuration["AuthTokenExpiryTime:Hours"]);
                        DateTime expiresAt = DateTime.Now.AddHours(hours);
                        _tokenStore.AddToken(user.Id.ToString(), token, expiresAt);
                        return token;
                    }
                    else
                    {
                        return Messages.AuthenticationFailed;
                    }
                }
                else
                {
                    return Messages.AuthenticationFailed;
                }
            }
            else
            {
                Admin? admin = await _adminRepository.GetByEmailAsync(loginVM.Email);
                if (admin != null)
                {
                    string dbPassword = admin.PasswordHash;
                    if (_encryptDecryptServices.VerifyPassword(loginVM.Password, dbPassword))
                    {
                        string token = _jwtServices.GenerateJwtToken(admin.Id);
                        double hours = Convert.ToDouble(_configuration["AuthTokenExpiryTime:Hours"]);
                        DateTime expiresAt = DateTime.Now.AddHours(hours);
                        _tokenStore.AddToken(admin.Id.ToString(), token, expiresAt);
                        return token;
                    }
                    else
                    {
                        return Messages.AuthenticationFailed;
                    }
                }
                else
                {
                    return Messages.AuthenticationFailed;
                }
            }
        }

        public async Task<string> ForgotPassword(LoginVM loginVM)
        {
            if (loginVM.IsAdmin == false)
            {
                User? user = await _userRepository.GetByEmailAsync(loginVM.Email);
                if (user != null)
                {
                    string token = GenerateToken.GenerateGuid();
                    _emailServices.SendEmail(loginVM.Email, token, false);
                    user.PasswordResetToken = token;
                    user.PasswordResetTokenExpiry = DateTime.UtcNow.AddHours(1);
                    await _userRepository.UpdateAsync(user);
                    return Messages.SuccessMessage;
                }
                else
                {
                    return string.Empty;
                }
            }
            else
            {
                Admin? admin = await _adminRepository.GetByEmailAsync(loginVM.Email);
                if (admin != null)
                {
                    string token = GenerateToken.GenerateGuid();
                    _emailServices.SendEmail(loginVM.Email, token, true);
                    admin.PasswordResetToken = token;
                    admin.PasswordResetTokenExpiry = DateTime.UtcNow.AddHours(1);
                    await _adminRepository.UpdateAsync(admin);
                    return Messages.SuccessMessage;
                }
                else
                {
                    return string.Empty;
                }
            }

        }
        public async Task<string> ValidatePasswordResetToken(string token, bool isAdmin)
        {
            if (isAdmin == false)
            {
                User? user = await _userRepository.GetByPasswordResetToken(token);
                if (user != null && user.PasswordResetTokenExpiry > DateTime.UtcNow && !string.IsNullOrEmpty(token))
                {
                    return Messages.SuccessMessage;
                }
                else
                {
                    return string.Empty;
                }
            }
            else
            {
                Admin? admin = await _adminRepository.GetByResetPasswordToken(token);
                if (admin != null && admin.PasswordResetTokenExpiry > DateTime.UtcNow && !string.IsNullOrEmpty(token))
                {
                    return Messages.SuccessMessage;
                }
                else
                {
                    return string.Empty;
                }
            }


        }
        public async Task<string> ResetPassword(ResetPasswordVM resetPasswordVM)
        {
            if (resetPasswordVM.IsAdmin == false)
            {
                User? user = await _userRepository.GetByPasswordResetToken(resetPasswordVM.Token);
                if (user != null && !string.IsNullOrEmpty(resetPasswordVM.Token))
                {
                    string hashedPassword = _encryptDecryptServices.EncryptPassword(resetPasswordVM.NewPassword);
                    user.PasswordHash = hashedPassword;
                    user.PasswordResetToken = string.Empty;
                    user.PasswordResetTokenExpiry = DateTime.MinValue;
                    await _userRepository.UpdateAsync(user);
                    return Messages.SuccessMessage;
                }
                else
                {
                    return string.Empty;
                }
            }
            else
            {
                Admin? admin = await _adminRepository.GetByResetPasswordToken(resetPasswordVM.Token);
                if (admin != null && !string.IsNullOrEmpty(resetPasswordVM.Token))
                {
                    string hashedPassword = _encryptDecryptServices.EncryptPassword(resetPasswordVM.NewPassword);
                    admin.PasswordHash = hashedPassword;
                    admin.PasswordResetToken = string.Empty;
                    admin.PasswordResetTokenExpiry = DateTime.MinValue;
                    await _adminRepository.UpdateAsync(admin);
                    return Messages.SuccessMessage;
                }
                else
                {
                    return string.Empty;
                }
            }

        }

        public void AddInMemoryToken(string token)
        {
            int id = _jwtServices.GetUserId(token);
            double hours = Convert.ToDouble(_configuration["AuthTokenExpiryTime:Hours"]);
            DateTime expiresAt = DateTime.Now.AddHours(hours);
            _tokenStore.AddToken(id.ToString(), token, expiresAt);
        }
    }
}
