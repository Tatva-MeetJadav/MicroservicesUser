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
        foreach (var kvp in _tokens)
        {
            if (kvp.Value.ExpiresAt <= utcNow)
            {
                yield return (kvp.Value.UserId, kvp.Key, kvp.Value.ExpiresAt);
            }
        }
    }

    public IEnumerable<(string userId, string token, DateTime expiresAt)> GetTokens(Func<string, bool> predicate)
    {
        foreach (var kvp in _tokens)
        {
            if (predicate(kvp.Key))
            {
                yield return (kvp.Value.UserId, kvp.Key, kvp.Value.ExpiresAt);
            }
        }
    }
}