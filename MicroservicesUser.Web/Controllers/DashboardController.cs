using MicroservicesUser.BusinessLogic.Interfaces;
using MicroservicesUser.Common.ResourcesFiles;
using MicroservicesUser.Models.ViewModels;
using MicroservicesUser.Models.ViewModels.Dashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MicroservicesUser.Web.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IDashboardServices _dashboardServices;
        public DashboardController(IDashboardServices dashboardServices)
        {
            _dashboardServices = dashboardServices;
        }
        public async Task<IActionResult> Index()
        {
            string token = Request.Cookies["AuthToken"] ?? string.Empty;
            EmailVerificationDashboardVM dashboardVM = await _dashboardServices.GetEmailVerificationDashboard(token);
            return View(dashboardVM);
        }

        [HttpGet]
        public async Task<IActionResult> UserProfile()
        {
            string token = Request.Cookies["AuthToken"] ?? string.Empty;
            ProfileVM profileVM = await _dashboardServices.GetUserProfile(token);
            return View(profileVM);
        }

        [HttpPost]
        public async Task<IActionResult> UserProfile(ProfileVM profileVM, IFormFile profilePhoto)
        {
            string result = await _dashboardServices.EditUserProfile(profileVM, profilePhoto);
            if (result == Messages.SuccessMessage)
            {
                TempData["SuccessMessage"] = "Profile updated successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Something went wrong.";
                return View(profileVM);
            }
            return RedirectToAction("UserProfile", "Dashboard");
        }

        [HttpPost]
        public async Task<JsonResult> ChangePassword(ChangePasswordVM changePasswordVM)
        {
            string token = Request.Cookies["AuthToken"] ?? string.Empty;
            string result = await _dashboardServices.ChangePassword(changePasswordVM, token);
            if (result == Messages.SuccessMessage)
            {
                return Json(Messages.SuccessMessage);
            }
            else if (result == Messages.WrongPassword)
            {
                return Json(Messages.WrongPassword);
            }
            else
            {
                return Json(Messages.Failed);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetProfilePhoto()
        {
            string token = Request.Cookies["AuthToken"] ?? string.Empty;
            string profilePhotoUrl = await _dashboardServices.GetProfilePhoto(token);
            return Json(profilePhotoUrl);
        }

        [HttpGet]
        public async Task<IActionResult> GetChartData(string range)
        {
            string token = Request.Cookies["AuthToken"] ?? string.Empty;
            EmailVerificationChart result = await _dashboardServices.GetChartData(token, range);
            return Json(result);
        }
    }
}
