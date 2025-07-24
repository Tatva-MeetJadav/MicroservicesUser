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

        [Authorize(Roles = "User")]
        public async Task<IActionResult> Index()
        {
            string token = Request.Cookies["AuthToken"] ?? string.Empty;
            EmailVerificationDashboardVM dashboardVM = await _dashboardServices.GetEmailVerificationDashboard(token);
            return View(dashboardVM);
        }

        [Authorize(Roles = "User")]
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
                return RedirectToAction("UserProfile", "Dashboard");
            }
            if (result == Messages.DuplicateUsername)
            {
                TempData["ErrorMessage"] = "Username already taken!";
                return RedirectToAction("UserProfile", "Dashboard");
            }
            else
            {
                TempData["ErrorMessage"] = "Something went wrong.";
                return View(profileVM);
            }
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

        [Authorize(Roles = "Admin,SupportAdmin")]
        public async Task<IActionResult> AdminDashboard()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> AdminProfile()
        {
            string token = Request.Cookies["AuthToken"] ?? string.Empty;
            ProfileVM profileVM = await _dashboardServices.GetAdminProfile(token);
            return View(profileVM);
        }

        [Authorize(Roles = "Admin,SupportAdmin")]
        [HttpPost]
        public async Task<IActionResult> AdminProfile(ProfileVM profileVM, IFormFile profilePhoto)
        {
            string result = await _dashboardServices.EditAdminProfile(profileVM, profilePhoto);
            if (result == Messages.SuccessMessage)
            {
                TempData["SuccessMessage"] = "Profile updated successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Something went wrong.";
                return View(profileVM);
            }
            return RedirectToAction("AdminProfile", "Dashboard");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<JsonResult> AdminChangePassword(ChangePasswordVM changePasswordVM)
        {
            string token = Request.Cookies["AuthToken"] ?? string.Empty;
            string result = await _dashboardServices.AdminChangePassword(changePasswordVM, token);
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

        [Authorize(Roles = "SupportAdmin")]
        [HttpGet]
        public async Task<ActionResult> GetUnreadNotifications()
        {
            AdminNotificationListVM result = await _dashboardServices.GetUnreadNotificationList();
            return PartialView("_AdminNotification", result);
        }

        [Authorize(Roles = "SupportAdmin")]
        [HttpPost]
        public async Task<IActionResult> ReadAllNotifications()
        {
            await _dashboardServices.ReadAllNotifications();
            return PartialView("_AdminNotification", new AdminNotificationListVM());
        }

    }
}
