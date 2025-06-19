using MicroservicesUser.Models.Models;
using MicroservicesUser.Models.ViewModels;
using Microsoft.AspNetCore.Http;

namespace MicroservicesUser.BusinessLogic.Interfaces
{
    public interface IDashboardServices
    {
        Task<ProfileVM> GetUserProfile(string token);
        Task<string> EditUserProfile(ProfileVM profileVM, IFormFile file);
        Task<string> ChangePassword(ChangePasswordVM changePasswordVM, string token);
    }
}