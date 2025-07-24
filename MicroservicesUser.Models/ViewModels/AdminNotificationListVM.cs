namespace MicroservicesUser.Models.ViewModels
{
    public class AdminNotificationListVM
    {
        public List<AdminNotificationVM> AdminNotificationList { get; set; } = new();
        public int TotalCount { get; set; }
    }

}