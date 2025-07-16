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
            (string userId, string token, DateTime expiresAt)? nextToken = _tokenStore.GetNextExpiringToken();
            if (nextToken != null)
            {
                (string userId, string token, DateTime expiresAt) = nextToken.Value;
                DateTime now = DateTime.UtcNow.ToLocalTime();
                if (expiresAt <= now)
                {
                    await _hubContext.Clients.User(userId).SendAsync("ForceLogout", cancellationToken: stoppingToken);
                    _tokenStore.RemoveToken(token);
                    continue;
                }
                else
                {
                    TimeSpan delay = expiresAt - now + TimeSpan.FromSeconds(2);
                    await Task.Delay(delay, stoppingToken);
                }
            }
            else
            {
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }
    }
}