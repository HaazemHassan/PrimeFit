using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PrimeFit.Application.Common.DTOS;
using PrimeFit.Application.ServicesContracts.Infrastructure;

namespace PrimeFit.Infrastructure.Notifications.Push
{
    public class FirebasePushNotificationService : IPushNotificationService
    {
        private readonly ILogger<FirebasePushNotificationService> _logger;

        public FirebasePushNotificationService(
            IOptions<FirebaseOptions> firebaseOptions,
            ILogger<FirebasePushNotificationService> logger)
        {
            _logger = logger;

            if (FirebaseApp.DefaultInstance is null)
            {
                var base64Credentials = firebaseOptions.Value.ServiceAccountKeyBase64;

                if (!string.IsNullOrWhiteSpace(base64Credentials))
                {
                    try
                    {
                        var jsonCredentialsBytes = Convert.FromBase64String(base64Credentials);
                        var jsonCredentials = System.Text.Encoding.UTF8.GetString(jsonCredentialsBytes);
                        var credential = CredentialFactory.FromJson<ServiceAccountCredential>(jsonCredentials).ToGoogleCredential();

                        FirebaseApp.Create(new AppOptions()
                        {
                            Credential = credential
                        });

                        _logger.LogInformation("Firebase App initialized successfully.");
                    }
                    catch (FormatException ex)
                    {
                        _logger.LogError(ex, "Firebase App initialization failed: ServiceAccountKeyBase64 is not a valid Base64 string.");
                    }
                }
                else
                {
                    _logger.LogWarning("Firebase App initialization failed: ServiceAccountKeyBase64 is missing or empty.");
                }
            }
        }


        public async Task<bool> SendToDeviceAsync(PushNotificationRequest request, string deviceToken, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(deviceToken))
            {
                _logger.LogWarning("Cannot send push notification: DeviceToken is null or empty.");
                return false;
            }

            var message = CreateMessage(request);
            message.Token = deviceToken;

            try
            {
                var response = await FirebaseMessaging.DefaultInstance.SendAsync(message, ct);
                _logger.LogInformation("Push notification sent successfully. MessageId: {MessageId}", response);
                return true;
            }
            catch (FirebaseMessagingException ex)
            {
                _logger.LogError(ex, "Failed to send push notification to device. Token: {Token}, Error: {ErrorCode}",
                    MaskToken(deviceToken), ex.MessagingErrorCode);
                return false;
            }
        }


        public async Task<int> SendToDevicesAsync(PushNotificationRequest request, IEnumerable<string> deviceTokens, CancellationToken ct = default)
        {
            var tokens = deviceTokens?.Where(t => !string.IsNullOrWhiteSpace(t)).ToList();

            if (tokens is null || tokens.Count == 0)
            {
                _logger.LogWarning("Cannot send push notifications: No valid device tokens provided.");
                return 0;
            }

            var message = new MulticastMessage
            {
                Tokens = tokens,
                Notification = new Notification
                {
                    Title = request.Title,
                    Body = request.Body,
                    ImageUrl = request.ImageUrl
                },
                Data = request.Data
            };

            try
            {
                var response = await FirebaseMessaging.DefaultInstance.SendEachForMulticastAsync(message, ct);

                _logger.LogInformation(
                    "Multicast push notification sent. Success: {SuccessCount}, Failure: {FailureCount}",
                    response.SuccessCount, response.FailureCount);

                if (response.FailureCount > 0)
                {
                    for (int i = 0; i < response.Responses.Count; i++)
                    {
                        if (!response.Responses[i].IsSuccess)
                        {
                            _logger.LogWarning(
                                "Failed to send to token at index {Index}. Error: {Error}",
                                i, response.Responses[i].Exception?.MessagingErrorCode);
                        }
                    }
                }

                return response.SuccessCount;
            }
            catch (FirebaseMessagingException ex)
            {
                _logger.LogError(ex, "Failed to send multicast push notification. Error: {ErrorCode}", ex.MessagingErrorCode);
                return 0;
            }
        }


        public async Task<bool> SendToTopicAsync(PushNotificationRequest request, string topic, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(topic))
            {
                _logger.LogWarning("Cannot send push notification: Topic is null or empty.");
                return false;
            }

            var message = CreateMessage(request);
            message.Topic = topic;

            try
            {
                var response = await FirebaseMessaging.DefaultInstance.SendAsync(message, ct);
                _logger.LogInformation("Push notification sent to topic '{Topic}'. MessageId: {MessageId}", topic, response);
                return true;
            }
            catch (FirebaseMessagingException ex)
            {
                _logger.LogError(ex, "Failed to send push notification to topic '{Topic}'. Error: {ErrorCode}", topic, ex.MessagingErrorCode);
                return false;
            }
        }


        #region Private Helpers

        private static Message CreateMessage(PushNotificationRequest request)
        {
            return new Message
            {
                Notification = new Notification
                {
                    Title = request.Title,
                    Body = request.Body,
                    ImageUrl = request.ImageUrl
                },
                Data = request.Data
            };
        }

        private static string MaskToken(string token)
        {
            if (token.Length <= 10)
                return "***";

            return string.Concat(token.AsSpan(0, 5), "...", token.AsSpan(token.Length - 5));
        }

        #endregion
    }
}
