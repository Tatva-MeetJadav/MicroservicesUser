using MicroservicesUser.BusinessLogic.Interfaces;
using MicroservicesUser.Models.ViewModels.Dashboard;
using Microsoft.AspNetCore.Mvc;

namespace MicroservicesUser.Web.Controllers
{
    public class ProxyVpnDetectionController : Controller
    {
        private readonly IProxyVpnDetectionServices _proxyVpnDetectionServices;
        public ProxyVpnDetectionController(IProxyVpnDetectionServices proxyVpnDetectionServices)
        {
            _proxyVpnDetectionServices = proxyVpnDetectionServices;
        }
        public async Task<IActionResult> Index()
        {
            ProxyVpnDetectionDashboardVM dashboardVM = await _proxyVpnDetectionServices.GetProxyVpnDetectionDashboard(null);
            return View(dashboardVM);
        }

        public async Task<IActionResult> GetDashboardData(List<int> userIds)
        {
            ProxyVpnDetectionDashboardVM dashboardVM = await _proxyVpnDetectionServices.GetProxyVpnDetectionDashboard(userIds);
            return PartialView("_ProxyVpnDetection", dashboardVM);
        }
    }
}