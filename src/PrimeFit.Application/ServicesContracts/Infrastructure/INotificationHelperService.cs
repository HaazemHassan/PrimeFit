using PrimeFit.Application.Common.DTOS;
using PrimeFit.Domain.Common.Enums;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.ServicesContracts.Infrastructure
{
    public interface INotificationHelperService
    {
        Task<UserNotification> AddNotificationAsync(int userId, string title, string message, NotificationType notificationType, CancellationToken ct = default);
        Task PushRealTimeAsync(int userId, InAppNotificationDto dto);
    }
}
