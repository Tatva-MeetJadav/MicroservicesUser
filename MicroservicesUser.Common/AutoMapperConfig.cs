
using AutoMapper;
using MicroservicesUser.Models.DTO;
using MicroservicesUser.Models.Models;
using MicroservicesUser.Models.ViewModels;
using MicroservicesUser.Models.ViewModels.Dashboard;
using MicroservicesUser.Models.ViewModels.History;

namespace MicroservicesUser.Common
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            //Authentication
            CreateMap<RegisterVM, User>()
               .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password))
               .ForMember(dest => dest.Id, opt => opt.Ignore())
               .ForMember(dest => dest.PasswordResetToken, opt => opt.Ignore())
               .ForMember(dest => dest.PasswordResetTokenExpiry, opt => opt.Ignore())
               .ForMember(dest => dest.Address, opt => opt.Ignore())
               .ForMember(dest => dest.MobileNumber, opt => opt.Ignore())
               .ForMember(dest => dest.ProfilePhotoGeneratedName, opt => opt.Ignore());

            //User Profile
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

            //Admin Profile
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

            //Userlist
            CreateMap<User, UserVM>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<PaginationDTO, UserListVM>();

            //HelpAndSupport
            CreateMap<User, HelpAndSupportVM>()
               .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.MobileNumber));
            CreateMap<HelpAndSupportVM, HelpAndSupport>()
               .ForMember(dest => dest.UserId, opt => opt.Ignore());
            CreateMap<HelpAndSupport, HelpAndSupportVM>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User!.Username));
            CreateMap<PaginationDTO, HelpAndSupportListVM>();


            //Log
            CreateMap<Log, LogVM>();
            CreateMap<PaginationDTO, LogListVM>();

            //EmailVerification
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
            CreateMap<PaginationDTO, AdminEmailVerificationDashboardDTO>();

        }
    }
}
