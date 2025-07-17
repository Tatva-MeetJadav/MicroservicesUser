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
            CreateMap<DashboardUserDTO, DashboardUserVM>();

            //ProxyVpnMapping
            CreateMap<ProxyVpnDetectionDashboardDTO, ProxyVpnDetectionDashboardVM>();
            CreateMap<ProxyVpnDetectionHistoryListDTO, ProxyVpnDetectionHistoryListVM>();
            CreateMap<ContinentIPStatsDTO, ContinentIPStatsVM>();

            //AdminEmailVerification
            CreateMap<AdminEmailVerificationDashboardDTO, AdminEmailVerificationDashboardVM>();
            CreateMap<EmailVerificationDateTimeStatesDTO, EmailVerificationDateTimeStatesVM>();
            CreateMap<AdminEmailVerificationHistoryListDTO, AdminEmailVerificationHistoryListVM>();
        }
    }
}
