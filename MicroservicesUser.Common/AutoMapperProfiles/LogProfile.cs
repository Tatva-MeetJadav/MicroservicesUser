using AutoMapper;
using MicroservicesUser.Models.DTO;
using MicroservicesUser.Models.Models;
using MicroservicesUser.Models.ViewModels;

namespace MicroservicesUser.Common.AutoMapperProfiles
{
    public class LogProfile : Profile
    {
        public LogProfile()
        {
            CreateMap<Log, LogVM>();
            CreateMap<PaginationDTO, LogListVM>();
        }
    }
}
