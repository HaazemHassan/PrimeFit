using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace PrimeFit.Infrastructure.Notifications.InApp
{
    /// <summary>
    /// Custom SignalR user ID provider that extracts the user ID from JWT claims.
    /// Maps to ClaimTypes.NameIdentifier which contains the DomainUserId.
    /// </summary>
    public class SignalRUserIdProvider : IUserIdProvider
    {
        public string? GetUserId(HubConnectionContext connection)
        {
            return connection.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
