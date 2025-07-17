using MicroservicesUser.Models.ViewModels.Dashboard;
using Microsoft.AspNetCore.Mvc;

namespace MicroservicesUser.Web.Controllers
{
    public class AdminEmailVerificationController : Controller
    {
        public IActionResult Index()
        {
            return View(new AdminEmailVerificationDashboardVM());
        }
    }
}