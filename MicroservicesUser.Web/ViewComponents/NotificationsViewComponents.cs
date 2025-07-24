using MicroservicesUser.BusinessLogic.Interfaces;
using MicroservicesUser.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace MicroservicesUser.Web.ViewComponents
{
    public class NotificationsViewComponent : ViewComponent
    {
        private readonly IDashboardServices _dashboardServices;

        public NotificationsViewComponent(IDashboardServices dashboardServices)
        {
            _dashboardServices = dashboardServices;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            AdminNotificationListVM notifications = await _dashboardServices.GetUnreadNotificationList();
            return View("/Views/Shared/_AdminNotification.cshtml", notifications);
        }
    }
}