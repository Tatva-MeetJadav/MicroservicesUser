using MicroservicesUser.BusinessLogic.Interfaces;
using MicroservicesUser.Models.DTO;
using MicroservicesUser.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MicroservicesUser.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly IUserServices _userServices;
        public UserController(IUserServices userServices)
        {
            _userServices = userServices;
        }

        public async Task<IActionResult> Index()
        {
            PaginationDTO paginationDTO = new();
            UserListVM result = await _userServices.GetUserList(paginationDTO);
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> GetUserList([FromBody] PaginationDTO paginationDTO)
        {
            UserListVM result = await _userServices.GetUserList(paginationDTO);
            return PartialView("_UserList", result);
        }

        [HttpPost]
        public async Task<IActionResult> BlockUnblockUser(string id)
        {
            UserVM result = await _userServices.BlockUnblockUser(id);
            return PartialView("_UserRow", result);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            UserVM result = await _userServices.DeleteUser(id);
            return PartialView("_UserRow", result);
        }
    }
}