using MicroservicesUser.BusinessLogic.Interfaces;
using MicroservicesUser.Models.DTO;
using MicroservicesUser.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MicroservicesUser.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class LogController : Controller
    {
        private readonly ILogServices _logServices;
        public LogController(ILogServices logServices)
        {
            _logServices = logServices;
        }
        public async Task<IActionResult> Index()
        {
            LogListVM result = await _logServices.GetLogList(new PaginationDTO());
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> GetLogList([FromBody] PaginationDTO paginationDTO)
        {
            LogListVM result = await _logServices.GetLogList(paginationDTO);
            return PartialView("_LogList", result);
        }
    }
}