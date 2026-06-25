using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using PrimeFit.Application.Common.DTOS;
using PrimeFit.Application.ServicesContracts.Infrastructure;

namespace PrimeFit.Infrastructure.Notifications.InApp
{
    internal class InAppNotificationService : IInAppNotificationService
    {
        private readonly IHubContext<NotificationHub, INotificationClient> _hubContext;
        private readonly ILogger<InAppNotificationService> _logger;

        public InAppNotificationService(
            IHubContext<NotificationHub, INotificationClient> hubContext,
            ILogger<InAppNotificationService> logger)
        {
            _hubContext = hubContext;
            _logger = logger;
        }

        public async Task SendToUserAsync(int userId, InAppNotificationDto notification)
        {
            _logger.LogInformation(
                "Sending in-app notification to user {UserId}. Title: {Title}, Type: {NotificationType}",
                userId, notification.Title, notification.NotificationType);

            await _hubContext.Clients
                .User(userId.ToString())
                .ReceiveNotification(notification);
        }

        public async Task SendToGroupAsync(string groupName, InAppNotificationDto notification)
        {
            _logger.LogInformation(
                "Sending in-app notification to group {GroupName}. Title: {Title}, Type: {NotificationType}",
                groupName, notification.Title, notification.NotificationType);

            await _hubContext.Clients
                .Group(groupName)
                .ReceiveNotification(notification);
        }
    }
}
