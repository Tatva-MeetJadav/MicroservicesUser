using AutoMapper;
using MicroservicesUser.BusinessLogic.Interfaces;
using MicroservicesUser.DataAccess.Repository.Interfaces;
using MicroservicesUser.Models.DTO;
using MicroservicesUser.Models.Models;
using MicroservicesUser.Models.ViewModels;
using MicroservicesUser.Models.ViewModels.Dashboard;
using Newtonsoft.Json;


namespace MicroservicesUser.BusinessLogic.Implementations
{
    public class ProxyVpnDetectionServices : IProxyVpnDetectionServices
    {
        private readonly IDashboardRepository _dashboardRepository;
        private readonly IProxyVpnDetectionRepository _proxyVpnDetectionRepository;
        private readonly IMapper _mapper;
        public ProxyVpnDetectionServices(IDashboardRepository dashboardRepository, IMapper mapper, IProxyVpnDetectionRepository proxyVpnDetectionRepository)
        {
            _dashboardRepository = dashboardRepository;
            _mapper = mapper;
            _proxyVpnDetectionRepository = proxyVpnDetectionRepository;
        }

        public async Task<ProxyVpnDetectionDashboardVM> GetProxyVpnDetectionDashboard(List<int>? userIds)
        {
            ProxyVpnDetectionDashboardDTO dashboardDTO = await _dashboardRepository.GetProxyVpnDetectionDashboard(userIds);
            ProxyVpnDetectionDashboardVM dashboardVM = _mapper.Map<ProxyVpnDetectionDashboardVM>(dashboardDTO);
            return dashboardVM;
        }

        public async Task<ProxyVpnDetectionDashboardVM> GetProxyVpnDetectionHistoryList(ProxyVpnDetectionHistoryRequestDTO requestDto)
        {
            ProxyVpnDetectionDashboardDTO resultdto = await _proxyVpnDetectionRepository.GetListAsync(requestDto);
            ProxyVpnDetectionDashboardVM dashboardVM = _mapper.Map<ProxyVpnDetectionDashboardVM>(resultdto);
            return dashboardVM;
        }

        public async Task<ProxyVpnDetectionViewDetailVM> GetProxyVpnDetectionViewDetail(int id)
        {
            ProxyVpnDetection? proxyVpnDetection = await _proxyVpnDetectionRepository.GetAsync(id);
            ProxyVpnDetectionViewDetailVM viewDetailVM = JsonConvert.DeserializeObject<ProxyVpnDetectionViewDetailVM>
            (proxyVpnDetection!.ProxyVpnResponseParam.RootElement.GetRawText()) ?? new ProxyVpnDetectionViewDetailVM();
            return viewDetailVM;
        }
    }
}
