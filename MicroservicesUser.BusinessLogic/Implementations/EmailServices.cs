using System.Net;
using System.Net.Mail;
using Microservices.BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Microservices.BusinessLogic.Implementations
{
    public class EmailServices : IEmailServices
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public EmailServices(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }
        public void SendEmail(string email, string token)
        {
            string subject = "Regarding Forgot Password";
            SmtpClient client = new(_configuration["EmailConfiguration:Host"], Convert.ToInt16(_configuration["EmailConfiguration:Port"]));
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(_configuration["EmailConfiguration:SenderEmail"], _configuration["EmailConfiguration:Password"]);

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(_configuration["EmailConfiguration:SenderEmail"] ?? string.Empty);
            mailMessage.IsBodyHtml = true;
            mailMessage.To.Add(email);
            string resetLink = _configuration["ResetPasswordLink:Route"] + "?token=" + token;
            string path = Path.Combine(_env.WebRootPath, "Templates", "ResetPasswordLinkEmail.html");

            if (File.Exists(path))
            {
                string emailBody = File.ReadAllText(path);
                mailMessage.Body = emailBody.Replace("{{ResetPasswordLink}}", resetLink);
            }
            mailMessage.Subject = subject;
            client.Send(mailMessage);
        }
    }
}