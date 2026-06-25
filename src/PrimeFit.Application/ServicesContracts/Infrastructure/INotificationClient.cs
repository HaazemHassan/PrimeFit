using PrimeFit.Application.Common.DTOS;

namespace PrimeFit.Application.ServicesContracts.Infrastructure
{
    /// <summary>
    /// Strongly-typed SignalR hub client interface for in-app notifications.
    /// Defines the methods that the server can invoke on connected clients.
    /// </summary>
    public interface INotificationClient
    {
        Task ReceiveNotification(InAppNotificationDto notification);
    }
}
