using AutoMapper;
using MicroservicesUser.BusinessLogic.Interfaces;
using MicroservicesUser.BusinessLogic.Utilities;
using MicroservicesUser.DataAccess.Repository.Interfaces;
using MicroservicesUser.Models.Models;
using MicroservicesUser.Models.ViewModels;
using Microservices.Common.ResourcesFiles;

namespace MicroservicesUser.BusinessLogic.Implementations
{
    public class AuthenticationServices : IAuthenticationServices
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IJwtServices _jwtServices;
        public AuthenticationServices(IMapper mapper, IUserRepository userRepository,IJwtServices jwtServices)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _jwtServices = jwtServices;
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
            string hashedPassword = EncryptDecrypt.EncryptPassword(loginVM.Password);
            User? user = await _userRepository.GetByEmailAsync(loginVM.Email);
            if(user != null)
            {
                string dbPassword = user.PasswordHash;
                if(EncryptDecrypt.VerifyPassword(dbPassword,hashedPassword))
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
    }
}
