namespace Microservices.BusinessLogic.Interfaces
{
    public interface IEmailServices
    {
        public void SendEmail(string email, string token, bool isAdmin);
    }
}