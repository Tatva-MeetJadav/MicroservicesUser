
using MicroservicesUser.BusinessLogic.Interfaces;
using MicroservicesUser.Models.DTO;
using MicroservicesUser.Models.ViewModels.History;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MicroservicesUser.Web.Controllers
{
    [Authorize(Roles = "User")]
    public class HistoryController : Controller
    {
        private readonly IEmailVerificationServices _emailVerificationServices;
        public HistoryController(IEmailVerificationServices emailVerificationServices)
        {
            _emailVerificationServices = emailVerificationServices;
        }
        public async Task<IActionResult> Index()
        {
            string token = Request.Cookies["AuthToken"] ?? string.Empty;
            EmailVerificationListHistoryVM result = await _emailVerificationServices.GetEmailVerificationListHistory(new PaginationDTO(), token);
            return View(result);
        }

        public async Task<IActionResult> GetEmailVerificationHistoryList([FromBody] PaginationDTO paginationVM)
        {
            string token = Request.Cookies["AuthToken"] ?? string.Empty;
            EmailVerificationListHistoryVM result = await _emailVerificationServices.GetEmailVerificationListHistory(paginationVM, token);
            return PartialView("_EmailVerificationHistory", result);
        }

        public async Task<IActionResult> EmailVerificationDetailedHistory(string id)
        {
            EmailVerificationDetailVM result = await _emailVerificationServices.GetEmailDetailedHistory(id);
            return View(result);
        }

        public async Task<ActionResult> ExportEmailVerificationDetail(string id)
        {
            (byte[] fileContents, string fileName) = await _emailVerificationServices.ExportEmailDetailHistory(id);
            return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        [HttpPost]
        public async Task<ActionResult> ExportEmailVerificationHistoryList([FromBody] PaginationDTO paginationDTO)
        {
            string token = Request.Cookies["AuthToken"] ?? string.Empty;
            (byte[] fileContents, string fileName) = await _emailVerificationServices.ExportEmailVerificationHistoryList(paginationDTO, token);
            string base64String = Convert.ToBase64String(fileContents);
            return Json(new { fileContents = base64String, fileName });
        }
    }
}