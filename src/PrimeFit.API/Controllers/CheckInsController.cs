using Microsoft.AspNetCore.Mvc;
using PrimeFit.Application.Features.Attendance.Commands.CreateCheckIn;

namespace PrimeFit.API.Controllers
{
    public class CheckInsController : BaseController
    {

        [HttpPost]
        public async Task<IActionResult> CheckInCustomer([FromBody] CreateCheckInCommand command)
        {
            var result = await Mediator.Send(command);


            if (result.IsError)
            {
                return Problem(result.Errors);

            }
            return Ok(result.Value);

        }

    }
}
