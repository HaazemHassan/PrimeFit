using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using PrimeFit.Application.ServicesContracts.Infrastructure;
using System.Security.Claims;

namespace PrimeFit.Infrastructure.Notifications.InApp
{
    [Authorize]
    public class NotificationHub : Hub<INotificationClient>
    {
        private readonly ILogger<NotificationHub> _logger;

        public NotificationHub(ILogger<NotificationHub> logger)
        {
            _logger = logger;
        }

        public override async Task OnConnectedAsync()
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogWarning("SignalR connection attempt with missing NameIdentifier claim. ConnectionId: {ConnectionId}", Context.ConnectionId);
                await base.OnConnectedAsync();
                return;
            }

            _logger.LogInformation(
                "User {UserId} connected to NotificationHub. ConnectionId: {ConnectionId}",
                userId, Context.ConnectionId);

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            _logger.LogInformation(
                "User {UserId} disconnected from NotificationHub. ConnectionId: {ConnectionId}",
                userId, Context.ConnectionId);

            await base.OnDisconnectedAsync(exception);
        }
    }
}
