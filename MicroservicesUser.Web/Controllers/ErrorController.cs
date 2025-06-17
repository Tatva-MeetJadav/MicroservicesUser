using Microsoft.AspNetCore.Mvc;

namespace MicroservicesUser.Web.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult ResetPasswordExpired()
        {
            return View();
        }
    }
}