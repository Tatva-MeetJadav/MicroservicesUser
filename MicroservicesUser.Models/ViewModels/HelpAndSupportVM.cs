namespace MicroservicesUser.Models.ViewModels
{
    public class HelpAndSupportVM : ProfileVM
    {
        public string Category { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        
    }
}