using MicroservicesUser.BusinessLogic.Interfaces;
using MicroservicesUser.Models.ViewModels.EmailVerification;
using Microsoft.AspNetCore.Mvc;

namespace MicroservicesUser.Web.Controllers
{
    public class EmailVerificationController : Controller
    {
        private readonly IEmailVerificationServices _emailVerificationServices;
        public EmailVerificationController(IEmailVerificationServices emailVerificationServices)
        {
            _emailVerificationServices = emailVerificationServices;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(EmailVerificationRequestVM emailVerificationVM)
        {
            EmailVerificationResponseVM result = await _emailVerificationServices.VerifyEmail(emailVerificationVM);
            return PartialView("_EmailVerificationResponse", result);
        }
    }
}
