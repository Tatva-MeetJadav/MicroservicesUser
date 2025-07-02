namespace MicroservicesUser.BusinessLogic.ServerStorage.Interfaces
{
    public interface ITokenStore
    {
        void AddToken(string userId, string token, DateTime expiresAt);
        void RemoveToken(string token);
        IEnumerable<(string userId, string token, DateTime expiresAt)> GetExpiredTokens(DateTime utcNow);
        (string userId, string token, DateTime expiresAt)? GetNextExpiringToken();
    }
}