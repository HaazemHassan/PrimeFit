using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrimeFit.API.Requests.Client.Users;
using PrimeFit.Application.Contracts.Api;
using PrimeFit.Application.Features.Subscriptions.Queries.GetMySubscriptions;
using PrimeFit.Application.Features.Users.Commands.UpdateProfile;
using PrimeFit.Application.Features.Users.Queries.CheckEmailAvailability;
using PrimeFit.Domain.Common.Enums;

namespace PrimeFit.API.Controllers
{
    public class UsersController(ICurrentUserService _currentUserService, IMapper _mapper) : BaseController
    {

        [HttpGet("check-email")]
        public async Task<IActionResult> CheckEmailAvailability([FromQuery] CheckEmailAvailabilityQuery query)
        {
            var result = await Mediator.Send(query);
            if (result.IsError)
                return Problem(result.Errors);
            return Ok(result.Value);
        }



        [HttpPatch("me")]
        [Authorize]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateMyPorfileRequest request)
        {
            var command = _mapper.Map<UpdateProfileCommand>(request);
            command.OwnerUserId = _currentUserService.UserId!.Value;

            var result = await Mediator.Send(command);

            if (result.IsError)
                return Problem(result.Errors);

            return Ok(result.Value);
        }


        [HttpGet("me/subscriptions")]
        [Authorize]
        public async Task<IActionResult> GetMySubscriptions([FromQuery] SubscriptionStatus? status)
        {
            var query = new GetMySubscriptionsQuery { Status = status };
            var result = await Mediator.Send(query);
            if (result.IsError)
            {
                return Problem(result.Errors);
            }
            return Ok(result.Value);
        }

    }
}
