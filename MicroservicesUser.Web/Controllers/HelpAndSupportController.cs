using MicroservicesUser.BusinessLogic.Interfaces;
using MicroservicesUser.Common.ResourcesFiles;
using MicroservicesUser.Models.DTO;
using MicroservicesUser.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MicroservicesUser.Web.Controllers
{
    public class HelpAndSupportController : Controller
    {
        private readonly IHelpAndSupportServices _helpAndSupportServices;
        public HelpAndSupportController(IHelpAndSupportServices helpAndSupportServices)
        {
            _helpAndSupportServices = helpAndSupportServices;
        }

        [Authorize(Roles = "User")]
        public async Task<IActionResult> Index()
        {
            string token = Request.Cookies["AuthToken"] ?? string.Empty;
            HelpAndSupportVM result = await _helpAndSupportServices.GetHelpAndSupportUserData(token);
            return View(result);
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        public async Task<IActionResult> HelpAndSupport(HelpAndSupportVM helpAndSupportVM)
        {
            string token = Request.Cookies["AuthToken"] ?? string.Empty;
            string result = await _helpAndSupportServices.AddHelpAndSupport(helpAndSupportVM, token);
            if (result == Messages.SuccessMessage)
            {
                TempData["SuccessMessage"] = "Request sent successfully.";
                return RedirectToAction("Index");
            }
            TempData["ErrorMessage"] = "Something went wrong!";
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> AdminSupport()
        {
            HelpAndSupportRequestDTO requestDTO = new();
            HelpAndSupportListVM result = await _helpAndSupportServices.GetHelpAndSupportList(requestDTO);
            return View(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> GetHelpAndSupportList([FromBody] HelpAndSupportRequestDTO requestDTO)
        {
            HelpAndSupportListVM result = await _helpAndSupportServices.GetHelpAndSupportList(requestDTO);
            return PartialView("_AdminSupportList", result);
        }
    }
}