using AutoMapper;
using MicroservicesUser.Models.DTO;
using MicroservicesUser.Models.Models;
using MicroservicesUser.Models.ViewModels;

namespace Microservices.Common.AutoMapperProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RegisterVM, User>()
               .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password))
               .ForMember(dest => dest.Id, opt => opt.Ignore())
               .ForMember(dest => dest.PasswordResetToken, opt => opt.Ignore())
               .ForMember(dest => dest.PasswordResetTokenExpiry, opt => opt.Ignore())
               .ForMember(dest => dest.Address, opt => opt.Ignore())
               .ForMember(dest => dest.MobileNumber, opt => opt.Ignore())
               .ForMember(dest => dest.ProfilePhotoGeneratedName, opt => opt.Ignore());

            CreateMap<User, ProfileVM>()
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.MobileNumber))
            .ForMember(dest => dest.ConfirmPassword, opt => opt.Ignore())
            .ForMember(dest => dest.ProfilePhotoName, opt => opt.Ignore())
            .ForMember(dest => dest.Password, opt => opt.Ignore());

            CreateMap<ProfileVM, User>()
                .ForMember(dest => dest.ProfilePhotoGeneratedName, opt => opt.Ignore())
                .ForMember(dest => dest.MobileNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Email, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordResetToken, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordResetTokenExpiry, opt => opt.Ignore());

            CreateMap<User, UserVM>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<PaginationDTO, UserListVM>();

        }
    }
}
