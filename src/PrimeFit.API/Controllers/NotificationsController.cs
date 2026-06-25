using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrimeFit.API.Requests.Notifications;
using PrimeFit.Application.Features.InAppNotifications.Commands.MarkNotificationAsRead;
using PrimeFit.Application.Features.InAppNotifications.Queries.GetMyUnreadNotifications;
using PrimeFit.Application.Features.Notifications.Commands.RegisterDeviceToken;

namespace PrimeFit.API.Controllers
{
    public class NotificationsController : BaseController
    {
        [HttpPost("register-token")]
        public async Task<IActionResult> RegisterToken([FromBody] RegisterDeviceTokenRequest request, CancellationToken ct)
        {
            var command = new RegisterDeviceTokenCommand(request.Token, request.DevicePlatform);

            var result = await Mediator.Send(command, ct);

            if (result.IsError)
            {
                return Problem(result.Errors);
            }

            return Ok();
        }

        [HttpGet("unread")]
        [Authorize]
        public async Task<IActionResult> GetMyUnreadNotifications(CancellationToken ct)
        {
            var query = new GetMyUnreadNotificationsQuery();

            var result = await Mediator.Send(query, ct);

            if (result.IsError)
            {
                return Problem(result.Errors);
            }

            return Ok(result.Value);
        }

        [HttpPut("{id:int}/read")]
        [Authorize]
        public async Task<IActionResult> MarkAsRead([FromRoute] int id, CancellationToken ct)
        {
            var command = new MarkNotificationAsReadCommand { NotificationId = id };

            var result = await Mediator.Send(command, ct);

            if (result.IsError)
            {
                return Problem(result.Errors);
            }

            return Ok();
        }
    }
}
