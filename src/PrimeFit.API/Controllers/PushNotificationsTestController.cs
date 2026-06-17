using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrimeFit.Application.Common.DTOS;
using PrimeFit.Application.ServicesContracts.Infrastructure;

namespace PrimeFit.API.Controllers
{
    [AllowAnonymous]
    public class PushNotificationsTestController : BaseController
    {
        private readonly IPushNotificationService _pushNotificationService;

        public PushNotificationsTestController(IPushNotificationService pushNotificationService)
        {
            _pushNotificationService = pushNotificationService;
        }

        [HttpPost("send-test")]
        public async Task<IActionResult> SendTestNotification([FromBody] string fcmToken, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(fcmToken))
            {
                return BadRequest("FCM Token is required.");
            }

            var request = new PushNotificationRequest
            {
                Title = "Prime Fit",
                Body = "neymar ya 5wl",
                Data = new Dictionary<string, string>
                {
                    ["testKey"] = "testValue",
                    ["timestamp"] = DateTime.UtcNow.ToString("O")
                }
            };

            var result = await _pushNotificationService.SendToDeviceAsync(request, fcmToken, ct);

            if (result)
            {
                return Ok(new { Message = "Notification sent successfully." });
            }

            return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Failed to send notification. Check logs for details." });
        }
    }
}
