using System.Collections.Concurrent;
using MicroservicesUser.BusinessLogic.ServerStorage.Interfaces;

public class InMemoryTokenStore : ITokenStore
{
    private readonly ConcurrentDictionary<string, (string UserId, DateTime ExpiresAt)> _tokens = new();

    public void AddToken(string userId, string token, DateTime expiresAt)
    {
        _tokens[token] = (userId, expiresAt);
    }

    public void RemoveToken(string token)
    {
        _tokens.TryRemove(token, out _);
    }

    public IEnumerable<(string userId, string token, DateTime expiresAt)> GetExpiredTokens(DateTime utcNow)
    {
        foreach (KeyValuePair<string, (string UserId, DateTime ExpiresAt)> kvp in _tokens)
        {
            if (kvp.Value.ExpiresAt <= utcNow)
            {
                yield return (kvp.Value.UserId, kvp.Key, kvp.Value.ExpiresAt);
            }
        }
    }

    public (string userId, string token, DateTime expiresAt)? GetNextExpiringToken()
    {
        KeyValuePair<string, (string UserId, DateTime ExpiresAt)> next = _tokens.OrderByDescending(kvp => kvp.Value.ExpiresAt).FirstOrDefault();
        if (next.Key != null)
        {
            (string userId, DateTime expiresAt) = next.Value;
            return (userId, next.Key, expiresAt);
        }
        return null;
    }
}