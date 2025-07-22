using MicroservicesUser.BusinessLogic.Interfaces;
using MicroservicesUser.Models.DTO;
using MicroservicesUser.Models.ViewModels;
using MicroservicesUser.Models.ViewModels.Dashboard;
using MicroservicesUser.Models.ViewModels.EmailVerification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MicroservicesUser.Web.Controllers
{
    [Authorize]
    public class EmailVerificationController : Controller
    {
        private readonly IEmailVerificationServices _emailVerificationServices;
        public EmailVerificationController(IEmailVerificationServices emailVerificationServices)
        {
            _emailVerificationServices = emailVerificationServices;
        }

        [Authorize(Roles = "User")]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        public async Task<IActionResult> Index(EmailVerificationRequestVM emailVerificationVM)
        {
            try
            {
                string token = Request.Cookies["AuthToken"] ?? string.Empty;
                EmailVerificationResponseVM result = await _emailVerificationServices.VerifyEmail(emailVerificationVM, token);
                return PartialView("_EmailVerificationResponse", result);
            }
            catch
            {
                return StatusCode(500, new { result = "failed" });
            }
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminEmailVerification()
        {
            AdminEmailVerificationDashboardVM dashboardVM = await _emailVerificationServices.GetAdminEmailVerificationDashboard(null);
            return View(dashboardVM);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> GetAdminDashboardData(List<int> userIds)
        {
            AdminEmailVerificationDashboardVM dashboardVM = await _emailVerificationServices.GetAdminEmailVerificationDashboard(userIds);
            return PartialView("_AdminEmailVerificationDashboard", dashboardVM);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> GetEmailVerificationHistory([FromBody] AdminEmailVerificationHistoryRequestDTO requestDTO)
        {
            AdminEmailVerificationDashboardVM dashboardVM = await _emailVerificationServices.GetAdminEmailVerificationHistoryList(requestDTO);
            return PartialView("_AdminEmailVerificationHistoryList", dashboardVM);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetChartData(List<int>? userIds, string range)
        {
            List<EmailVerificationDateTimeStatesVM> resulVm = await _emailVerificationServices.GetAdminEmailVerificationChart(userIds, range
            );
            return Json(resulVm);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAdminEmailVerificationViewDetail(int id)
        {
            AdminEmailVerificationDetailVM result = await _emailVerificationServices.GetAdminEmailVerificationDetail(id);
            return PartialView("_AdminEmailVerificationDetailedView", result);
        }
    }
}
