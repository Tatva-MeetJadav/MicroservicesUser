
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
        public IActionResult Index()
        {
            return View();
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

    }
}