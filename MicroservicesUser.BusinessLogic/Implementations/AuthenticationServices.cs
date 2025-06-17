using AutoMapper;
using MicroservicesUser.BusinessLogic.Interfaces;
using MicroservicesUser.BusinessLogic.Utilities;
using MicroservicesUser.DataAccess.Repository.Interfaces;
using MicroservicesUser.Models.Models;
using MicroservicesUser.Models.ViewModels;
using Microservices.BusinessLogic.Utilities;
using Microservices.BusinessLogic.Interfaces;
using MicroservicesUser.Common.ResourcesFiles;

namespace MicroservicesUser.BusinessLogic.Implementations
{
    public class AuthenticationServices : IAuthenticationServices
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IJwtServices _jwtServices;
        private readonly IEmailServices _emailServices;
        public AuthenticationServices(IMapper mapper, IUserRepository userRepository, IJwtServices jwtServices, IEmailServices emailServices)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _jwtServices = jwtServices;
            _emailServices = emailServices;
        }
        public async Task<string> RegisterUser(RegisterVM registerVM)
        {
            User? ifAlreadyExist = await _userRepository.GetByEmailAsync(registerVM.Email);
            if (ifAlreadyExist == null)
            {
                string passwordHash = EncryptDecrypt.EncryptPassword(registerVM.Password);
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
            User? user = await _userRepository.GetByEmailAsync(loginVM.Email);
            if (user != null)
            {
                string dbPassword = user.PasswordHash;
                if (EncryptDecrypt.VerifyPassword(loginVM.Password, dbPassword))
                {
                    string token = _jwtServices.GenerateJwtToken(loginVM.Email);
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

        public async Task<string> ForgotPassword(string email)
        {
            User? user = await _userRepository.GetByEmailAsync(email);
            if (user != null)
            {
                string token = GenerateToken.GenerateGuid();
                _emailServices.SendEmail(email, token);
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
        public async Task<string> ValidatePasswordResetToken(string token)
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
        public async Task<string> ResetPassword(ResetPasswordVM resetPasswordVM)
        {
            User? user = await _userRepository.GetByPasswordResetToken(resetPasswordVM.Token);
            if (user != null && !string.IsNullOrEmpty(resetPasswordVM.Token))
            {
                string hashedPassword = EncryptDecrypt.EncryptPassword(resetPasswordVM.NewPassword);
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
    }
}
