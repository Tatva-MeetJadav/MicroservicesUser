using MicroservicesUser.Models.ViewModels;
using MicroservicesUser.Models.ViewModels.Dashboard;
using Microsoft.AspNetCore.Http;

namespace MicroservicesUser.BusinessLogic.Interfaces
{
    public interface IDashboardServices
    {
        Task<ProfileVM> GetUserProfile(string token);
        Task<string> EditUserProfile(ProfileVM profileVM, IFormFile file);
        Task<string> ChangePassword(ChangePasswordVM changePasswordVM, string token);
        Task<EmailVerificationDashboardVM> GetEmailVerificationDashboard(string token);
        Task<string> GetProfilePhoto(string token);
        Task<EmailVerificationChart> GetChartData(string token, string range);
        Task<ProfileVM> GetAdminProfile(string token);
        Task<string> EditAdminProfile(ProfileVM profileVM, IFormFile formFile);
        Task<string> AdminChangePassword(ChangePasswordVM changePasswordVM, string token);
        Task<AdminNotificationListVM> GetUnreadNotificationList();
        Task ReadAllNotifications();
    }
}