using AutoMapper;
using MicroservicesUser.Models.DTO;
using MicroservicesUser.Models.ViewModels.Dashboard;
using MicroservicesUser.Models.ViewModels.History;

namespace MicroservicesUser.Common.AutoMapperProfiles
{
    public class DashboardProfile : Profile
    {
        public DashboardProfile()
        {
            CreateMap<EmailVerificationDashboardDTO, EmailVerificationDashboardVM>();
            CreateMap<EmailVerificationDTO, EmailVerificationHistoryVM>();
            CreateMap<ChartDTO, EmailVerificationChart>();

            //ProxyVpnMapping
            CreateMap<ProxyVpnDetectionDashboardDTO, ProxyVpnDetectionDashboardVM>();
            CreateMap<DashboardUserDTO, DashboardUserVM>();
            CreateMap<ProxyVpnDetectionHistoryListDTO, ProxyVpnDetectionHistoryListVM>();
        }
    }
}
