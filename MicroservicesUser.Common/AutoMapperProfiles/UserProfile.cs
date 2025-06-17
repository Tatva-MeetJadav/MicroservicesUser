using AutoMapper;
using MicroservicesUser.Models.Models;
using MicroservicesUser.Models.ViewModels;

namespace Microservices.Common.AutoMapperProfiles
{
    public class UserProfile:Profile
    {
        public UserProfile()
        {
            CreateMap<RegisterVM, User>()
               .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password))
               .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}
