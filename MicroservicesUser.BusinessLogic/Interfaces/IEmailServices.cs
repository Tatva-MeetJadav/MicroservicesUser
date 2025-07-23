namespace Microservices.BusinessLogic.Interfaces
{
    public interface IEmailServices
    {
        public void SendEmail(string email, string token, bool isAdmin);
        Task SendEmailAsync(string toEmail, string subject, string htmlContent);
    }
}