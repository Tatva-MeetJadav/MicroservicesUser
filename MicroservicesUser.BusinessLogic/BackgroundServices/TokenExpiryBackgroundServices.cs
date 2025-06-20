using MicroservicesUser.BusinessLogic.ServerStorage.Interfaces;
using MicroservicesUser.BusinessLogic.SignalRHubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;

public class TokenExpiryBackgroundService : BackgroundService
{
    private readonly IHubContext<LogoutHub> _hubContext;
    private readonly ITokenStore _tokenStore;

    public TokenExpiryBackgroundService(IHubContext<LogoutHub> hubContext, ITokenStore tokenStore)
    {
        _hubContext = hubContext;
        _tokenStore = tokenStore;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var expiredTokens = _tokenStore.GetExpiredTokens(DateTime.UtcNow.ToLocalTime()).ToList();
            var nullOrEmptyTokens = _tokenStore.GetTokens(string.IsNullOrEmpty).ToList();
            var tokensToLogout = expiredTokens.Concat(nullOrEmptyTokens);
            foreach (var tokenInfo in tokensToLogout)
            {
                await _hubContext.Clients.User(tokenInfo.userId).SendAsync("ForceLogout", cancellationToken: stoppingToken);
                _tokenStore.RemoveToken(tokenInfo.token);
            }

            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }
    }
}