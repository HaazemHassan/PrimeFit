using PrimeFit.Application.Common.DTOS;

namespace PrimeFit.Application.ServicesContracts.Infrastructure
{
    public interface IPushNotificationService
    {
        /// <summary>
        /// Sends a push notification to a single device using its FCM token.
        /// </summary>
        Task<bool> SendToDeviceAsync(PushNotificationRequest request, string deviceToken, CancellationToken ct = default);

        /// <summary>
        /// Sends a push notification to multiple devices using their FCM tokens.
        /// Returns the number of successfully sent notifications.
        /// </summary>
        Task<int> SendToDevicesAsync(PushNotificationRequest request, IEnumerable<string> deviceTokens, CancellationToken ct = default);

        /// <summary>
        /// Sends a push notification to all devices subscribed to a specific topic.
        /// </summary>
        Task<bool> SendToTopicAsync(PushNotificationRequest request, string topic, CancellationToken ct = default);
    }
}
