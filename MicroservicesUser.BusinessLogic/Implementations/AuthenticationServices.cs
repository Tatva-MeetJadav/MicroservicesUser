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
using MicroservicesUser.Models.DTO;
using System.Text.Json;
using Newtonsoft.Json;
using MicroservicesUser.Models.Enums;

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
        private readonly IGenericAPIClientServices _apiClient;
        private readonly IProxyVpnDetectionRepository _proxyVpnDetectionRepository;
        public AuthenticationServices(IMapper mapper, IUserRepository userRepository, IJwtServices jwtServices, IEmailServices emailServices, ITokenStore tokenStore, IConfiguration configuration, IEncryptDecryptServices encryptDecryptServices, IAdminRepository adminRepository, IGenericAPIClientServices apiClient, IProxyVpnDetectionRepository proxyVpnDetectionRepository)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _jwtServices = jwtServices;
            _emailServices = emailServices;
            _tokenStore = tokenStore;
            _configuration = configuration;
            _encryptDecryptServices = encryptDecryptServices;
            _adminRepository = adminRepository;
            _apiClient = apiClient;
            _proxyVpnDetectionRepository = proxyVpnDetectionRepository;
        }
        public async Task<string> RegisterUser(RegisterVM registerVM)
        {
            User? ifAlreadyExist = await _userRepository.GetByEmailAsync(registerVM.Email);
            if (ifAlreadyExist == null)
            {
                User? ifAlreadyExistWithUsername = await _userRepository.GetByUsernameAsync(registerVM.Username);
                if (ifAlreadyExistWithUsername == null)
                {
                    string passwordHash = _encryptDecryptServices.EncryptPassword(registerVM.Password);
                    registerVM.Password = passwordHash;
                    User user = _mapper.Map<User>(registerVM);
                    user.CreatedAt = DateTime.UtcNow.ToLocalTime();
                    user.UpdatedAt = DateTime.UtcNow.ToLocalTime();
                    await _userRepository.AddAsync(user);
                    return Messages.SuccessMessage;
                }
                else
                {
                    return Messages.DuplicateUsername;
                }
            }
            else
            {
                return Messages.DuplicateValue;
            }
        }

        public async Task<string> LoginUser(LoginVM loginVM, string ipAddress)
        {
            if (loginVM.IsAdmin == false)
            {
                User? user = await _userRepository.GetByEmailAsync(loginVM.Email);
                if (user != null)
                {
                    if (!user.IsBlocked)
                    {
                        string dbPassword = user.PasswordHash;
                        if (_encryptDecryptServices.VerifyPassword(loginVM.Password, dbPassword))
                        {
                            string token = _jwtServices.GenerateJwtToken(user.Id, Messages.UserRole);
                            double hours = Convert.ToDouble(_configuration["AuthTokenExpiryTime:Hours"]);
                            DateTime expiresAt = DateTime.Now.AddHours(hours);
                            _tokenStore.AddToken(user.Id.ToString(), token, expiresAt);
                            if (ipAddress == Messages.LocalIpAddress)
                            {
                                ipAddress = _configuration["IpAddress"]!;
                                ProxyAndVpnDetectionRequestDTO requestDTO = new() { IpAddress = ipAddress };
                                ProxyAndVpnDetectionResponseDTO responseDTO = new();
                                try
                                {
                                    responseDTO = await _apiClient.PostAsync<ProxyAndVpnDetectionRequestDTO, ProxyAndVpnDetectionResponseDTO>(requestDTO, _configuration["ProxyAndVpnDetectionAPI:Url"]!);
                                }
                                catch (Exception)
                                {
                                    ProxyVpnDetection proxyAndVpnDetectionFailed = new()
                                    {
                                        UserId = user.Id,
                                        CreatedAt = DateTime.UtcNow.ToLocalTime(),
                                        Status = Status.Success,
                                        ProxyVpnRequestParam = JsonDocument.Parse(JsonConvert.SerializeObject(requestDTO)),
                                        ProxyVpnResponseParam = JsonDocument.Parse(JsonConvert.SerializeObject(new ProxyAndVpnDetectionRequestDTO())),
                                    };
                                    await _proxyVpnDetectionRepository.AddAsync(proxyAndVpnDetectionFailed);
                                }
                                ProxyVpnDetection proxyAndVpnDetection = new()
                                {
                                    UserId = user.Id,
                                    CreatedAt = DateTime.UtcNow.ToLocalTime(),
                                    Status = Status.Success,
                                    ProxyVpnRequestParam = JsonDocument.Parse(JsonConvert.SerializeObject(requestDTO)),
                                    ProxyVpnResponseParam = JsonDocument.Parse(JsonConvert.SerializeObject(responseDTO)),
                                };
                                await _proxyVpnDetectionRepository.AddAsync(proxyAndVpnDetection);
                            }
                            return token;
                        }
                        else
                        {
                            return Messages.AuthenticationFailed;
                        }
                    }
                    else
                    {
                        return Messages.BlockedUser;
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
                        string token = _jwtServices.GenerateJwtToken(admin.Id, admin.Role.ToString());
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
