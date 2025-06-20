using MicroservicesUser.BusinessLogic.Interfaces;
using MicroservicesUser.Common.ResourcesFiles;
using MicroservicesUser.Models.ViewModels;
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
        public IActionResult Index()
        {
            return View();
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
    }
}
