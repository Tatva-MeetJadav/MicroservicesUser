using AutoMapper;
using Microservices.BusinessLogic.Interfaces;
using MicroservicesUser.BusinessLogic.Interfaces;
using MicroservicesUser.Common.ResourcesFiles;
using MicroservicesUser.DataAccess.Repository.Interfaces;
using MicroservicesUser.Models.DTO;
using MicroservicesUser.Models.Models;
using MicroservicesUser.Models.ViewModels;

namespace MicroservicesUser.BusinessLogic.Implementations
{
    public class HelpAndSupportServices : IHelpAndSupportServices
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtServices _jwtServices;
        private readonly IMapper _mapper;
        private readonly IHelpAndSupportRepository _helpAndSupportRepository;
        private readonly IEmailServices _emailServices;
        private readonly IViewRenderService _viewRenderService;
        private readonly IAdminRepository _adminRepository;
        public HelpAndSupportServices(IUserRepository userRepository, IJwtServices jwtServices, IMapper mapper, IHelpAndSupportRepository helpAndSupportRepository, IEmailServices emailServices, IViewRenderService viewRenderService, IAdminRepository adminRepository)
        {
            _userRepository = userRepository;
            _jwtServices = jwtServices;
            _mapper = mapper;
            _helpAndSupportRepository = helpAndSupportRepository;
            _emailServices = emailServices;
            _viewRenderService = viewRenderService;
            _adminRepository = adminRepository;
        }

        public async Task<string> AddHelpAndSupport(HelpAndSupportVM helpAndSupportVM, string token)
        {
            int id = _jwtServices.GetUserId(token);
            HelpAndSupport helpAndSupport = _mapper.Map<HelpAndSupport>(helpAndSupportVM);
            helpAndSupport.UserId = id;
            helpAndSupport.CreatedAt = DateTime.Now;
            helpAndSupportVM.CreatedAt = DateTime.Now;
            string AdminHtmlContent = await _viewRenderService.RenderToStringAsync("EmailTemplates/HelpAndSupport", helpAndSupportVM);
            string UserhtmlContent = await _viewRenderService.RenderToStringAsync("EmailTemplates/HelpAndSupportForUser", helpAndSupportVM);
            await _emailServices.SendEmailAsync(helpAndSupportVM.Email, "Regarding request summary", UserhtmlContent);
            List<Admin> supportAdmins = await _adminRepository.GetAdminListOfSupportRole();
            foreach (Admin admin in supportAdmins)
            {
                await _emailServices.SendEmailAsync(admin.Email, "Regarding new support request!", AdminHtmlContent);
            }
            await _helpAndSupportRepository.AddAsync(helpAndSupport);
            return Messages.SuccessMessage;
        }

        public async Task<HelpAndSupportVM> GetHelpAndSupportUserData(string token)
        {
            int id = _jwtServices.GetUserId(token);
            User? user = await _userRepository.GetByIdAsync(id);
            HelpAndSupportVM result = _mapper.Map<HelpAndSupportVM>(user);
            return result;
        }

        public async Task<HelpAndSupportListVM> GetHelpAndSupportList(HelpAndSupportRequestDTO requestDTO)
        {
            (List<HelpAndSupport> helpAndSupports, int totalCount) = await _helpAndSupportRepository.GetListAsync(requestDTO);
            List<HelpAndSupportVM> helpAndSupportVMs = _mapper.Map<List<HelpAndSupportVM>>(helpAndSupports);
            HelpAndSupportListVM helpAndSupportListVM = _mapper.Map<HelpAndSupportListVM>(requestDTO.PaginationDTO);
            helpAndSupportListVM.HelpAndSupport = helpAndSupportVMs;
            helpAndSupportListVM.TotalItems = totalCount;
            return helpAndSupportListVM;
        }

    }
}
