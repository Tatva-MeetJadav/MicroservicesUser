using Microservices.BusinessLogic.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace MicroservicesUser.BusinessLogic.Implementations
{
    public class EmailServices : IEmailServices
    {
        private readonly SmtpClient _smtpClient;
        private readonly string _fromAddress;

        public EmailServices(IConfiguration configuration)
        {
            string host = configuration["EmailConfiguration:Host"] ?? string.Empty;
            string senderEmail = configuration["EmailConfiguration:SenderEmail"] ?? string.Empty;
            string password = configuration["EmailConfiguration:Password"] ?? string.Empty;
            int port = int.Parse(configuration["EmailConfiguration:Port"] ?? "587");
            _fromAddress = senderEmail;
            _smtpClient = new SmtpClient(host, port)
            {
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false
            };
            if (!string.IsNullOrEmpty(password))
            {
                _smtpClient.Credentials = new NetworkCredential(senderEmail, password);
            }
        }

        public void SendEmail(string email, string token, bool isAdmin)
        {
            throw new NotImplementedException();
        }

        public async Task SendEmailAsync(string toEmail, string subject, string htmlContent)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(_fromAddress),
                Subject = subject,
                Body = htmlContent,
                IsBodyHtml = true,
            };
            mailMessage.To.Add(toEmail);

            await _smtpClient.SendMailAsync(mailMessage);
        }
    }
}
