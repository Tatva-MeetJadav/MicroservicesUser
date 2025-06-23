
using MicroservicesUser.BusinessLogic.Interfaces;
using MicroservicesUser.Models.ViewModels.History;
using Microsoft.AspNetCore.Mvc;

namespace MicroservicesUser.Web.Controllers
{
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

        public async Task<IActionResult> GetEmailVerificationHistoryList(string searchQuery, int page, int pageSize)
        {
            string token = Request.Cookies["AuthToken"] ?? string.Empty;
            EmailVerificationListHistoryVM result = await _emailVerificationServices.GetEmailVerificationListHistory(page, pageSize, searchQuery, token);
            return PartialView("_EmailVerificationHistory", result);
        }

    }
}