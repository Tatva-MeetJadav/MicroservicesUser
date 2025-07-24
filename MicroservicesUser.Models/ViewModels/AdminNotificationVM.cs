namespace MicroservicesUser.Models.ViewModels
{
    public class AdminNotificationVM
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Category = string.Empty;
        public DateTime CreatedAt { get; set; }
    }

}