
using MicroservicesUser.BusinessLogic.Interfaces;
using MicroservicesUser.Models.ViewModels;
using MicroservicesUser.Models.ViewModels.History;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MicroservicesUser.Web.Controllers
{
    [Authorize]
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

        public async Task<IActionResult> GetEmailVerificationHistoryList([FromBody] PaginationVM paginationVM)
        {
            string token = Request.Cookies["AuthToken"] ?? string.Empty;
            EmailVerificationListHistoryVM result = await _emailVerificationServices.GetEmailVerificationListHistory(paginationVM, token);
            return PartialView("_EmailVerificationHistory", result);
        }

    }
}