using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrimeFit.Application.Features.InAppNotifications.Commands.MarkNotificationAsRead;
using PrimeFit.Application.Features.InAppNotifications.Queries.GetMyNotifications;

namespace PrimeFit.API.Controllers
{
    public class InAppNotificationsController : BaseController
    {
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetMyNotifications([FromQuery] GetMyNotificationsQuery query, CancellationToken ct)
        {
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
