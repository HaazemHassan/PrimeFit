using Microsoft.AspNetCore.Mvc;
using PrimeFit.API.Requests.Notifications;
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
    }
}
