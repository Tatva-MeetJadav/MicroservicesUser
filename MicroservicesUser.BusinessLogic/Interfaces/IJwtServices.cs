namespace MicroservicesUser.BusinessLogic.Interfaces
{
    public interface IJwtServices
    {
        string GenerateJwtToken(string Email);
    }
}
