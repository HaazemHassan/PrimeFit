using Microsoft.Extensions.Logging;
using PrimeFit.Application.Common.DTOS;
using PrimeFit.Application.ServicesContracts.Infrastructure;
using PrimeFit.Domain.Common.Enums;
using PrimeFit.Domain.Entities;
using PrimeFit.Domain.RepositoriesContracts;

namespace PrimeFit.Infrastructure.Notifications.InApp
{
    internal class NotificationHelperService : INotificationHelperService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IInAppNotificationService _notificationService;
        private readonly ILogger<NotificationHelperService> _logger;

        public NotificationHelperService(
            IUnitOfWork unitOfWork,
            IInAppNotificationService notificationService,
            ILogger<NotificationHelperService> logger)
        {
            _unitOfWork = unitOfWork;
            _notificationService = notificationService;
            _logger = logger;
        }

        public async Task<UserNotification> AddNotificationAsync(int userId, string title, string message, NotificationType notificationType, CancellationToken ct = default)
        {
            var notification = new UserNotification(userId, title, message, notificationType);
            await _unitOfWork.UserNotifications.AddAsync(notification, ct);
            return notification;
        }

        public async Task PushRealTimeAsync(int userId, InAppNotificationDto dto)
        {
            try
            {
                await _notificationService.SendToUserAsync(userId, dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send real-time notification to user {UserId}.", userId);
            }
        }
    }
}
