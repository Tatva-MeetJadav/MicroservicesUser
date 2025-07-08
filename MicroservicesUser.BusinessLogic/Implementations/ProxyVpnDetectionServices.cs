using AutoMapper;
using MicroservicesUser.BusinessLogic.Interfaces;
using MicroservicesUser.DataAccess.Repository.Interfaces;
using MicroservicesUser.Models.DTO;
using MicroservicesUser.Models.ViewModels.Dashboard;
using Microsoft.Extensions.Configuration;


namespace MicroservicesUser.BusinessLogic.Implementations
{
    public class ProxyVpnDetectionServices : IProxyVpnDetectionServices
    {
        private readonly IConfiguration _configuration;
        private readonly IDashboardRepository _dashboardRepository;
        private readonly IMapper _mapper;
        public ProxyVpnDetectionServices(IConfiguration configuration, IDashboardRepository dashboardRepository, IMapper mapper)
        {
            _configuration = configuration;
            _dashboardRepository = dashboardRepository;
            _mapper = mapper;
        }

        public async Task<ProxyVpnDetectionDashboardVM> GetProxyVpnDetectionDashboard(List<int>? userIds)
        {
            ProxyVpnDetectionDashboardDTO dashboardDTO = await _dashboardRepository.GetProxyVpnDetectionDashboard(userIds);
            ProxyVpnDetectionDashboardVM dashboardVM = _mapper.Map<ProxyVpnDetectionDashboardVM>(dashboardDTO);
            return dashboardVM;
        }
    }

}
