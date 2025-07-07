using AutoMapper;
using MicroservicesUser.Models.Models;
using MicroservicesUser.Models.ViewModels;

namespace MicroservicesUser.Common.AutoMapperProfiles
{
    public class AdminProfile : Profile
    {
        public AdminProfile()
        {
            CreateMap<Admin, ProfileVM>()
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.MobileNumber))
            .ForMember(dest => dest.ConfirmPassword, opt => opt.Ignore())
            .ForMember(dest => dest.ProfilePhotoName, opt => opt.Ignore())
            .ForMember(dest => dest.Password, opt => opt.Ignore());

            CreateMap<ProfileVM, Admin>()
                .ForMember(dest => dest.ProfilePhotoGeneratedName, opt => opt.Ignore())
                .ForMember(dest => dest.MobileNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Email, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordResetToken, opt => opt.Ignore())
                .ForMember(dest => dest.Role, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordResetTokenExpiry, opt => opt.Ignore());
        }
    }
}
