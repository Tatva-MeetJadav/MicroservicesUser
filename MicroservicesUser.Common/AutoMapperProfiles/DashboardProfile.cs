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
            CreateMap<DashboardDTO, EmailVerificationDashboardVM>();
            CreateMap<EmailVerificationDTO, EmailVerificationHistoryVM>();
            CreateMap<ChartDTO, EmailVerificationChart>();
        }
    }
}
