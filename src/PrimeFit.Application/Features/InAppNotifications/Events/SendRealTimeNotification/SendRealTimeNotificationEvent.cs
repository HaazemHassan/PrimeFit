using PrimeFit.Application.Common.Messaging;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Features.InAppNotifications.Events.SendRealTimeNotification
{
    public record SendRealTimeNotificationEvent(UserNotification Notification) : IPostCommitEvent;
}
