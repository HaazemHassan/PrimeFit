using PrimeFit.Application.Common.DTOS;

namespace PrimeFit.Application.ServicesContracts.Infrastructure
{
    /// <summary>
    /// Abstraction for sending in-app notifications via real-time channels (e.g., SignalR).
    /// The core application logic depends only on this interface, not on any transport details.
    /// </summary>
    public interface IInAppNotificationService
    {
        /// <summary>
        /// Sends a real-time notification to a specific user by their ID.
        /// </summary>
        Task SendToUserAsync(int userId, InAppNotificationDto notification);

        /// <summary>
        /// Sends a real-time notification to all users in a specific group (e.g., branch admins).
        /// </summary>
        Task SendToGroupAsync(string groupName, InAppNotificationDto notification);
    }
}
