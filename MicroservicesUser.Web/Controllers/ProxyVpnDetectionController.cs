using MicroservicesUser.BusinessLogic.Interfaces;
using MicroservicesUser.Models.DTO;
using MicroservicesUser.Models.ViewModels;
using MicroservicesUser.Models.ViewModels.Dashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MicroservicesUser.Web.Controllers
{
    [Authorize(Roles = "Admin")]
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

        [HttpPost]
        public async Task<IActionResult> GetProxyVpnDetectionHistory([FromBody] ProxyVpnDetectionHistoryRequestDTO requestDto)
        {
            ProxyVpnDetectionDashboardVM result = await _proxyVpnDetectionServices.GetProxyVpnDetectionHistoryList(requestDto);
            return PartialView("_ProxyVpnDetectionHistoryList", result);
        }

        public async Task<IActionResult> GetProxyVpnViewDetail(int id)
        {
            ProxyVpnDetectionViewDetailVM viewDetailVM = await _proxyVpnDetectionServices.GetProxyVpnDetectionViewDetail(id);
            return PartialView("_ProxyVpnDetectionViewDetailModal", viewDetailVM);
        }

    }
}