namespace MicroservicesUser.BusinessLogic.Interfaces
{
    public interface IJwtServices
    {
        string GenerateJwtToken(int id, string role);
        int GetUserId(string token);
        string GetRole(string token);
        DateTime GetExpiryTime(string token);
    }
}
