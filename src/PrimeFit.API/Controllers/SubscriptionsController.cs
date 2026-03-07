using Microsoft.AspNetCore.Mvc;
using PrimeFit.API.Common.Constants;
using PrimeFit.Application.Features.Subscriptions.Commands.FreezeSubscription;
using PrimeFit.Application.Features.Subscriptions.Commands.UnfreezeSubscription;
using PrimeFit.Application.Features.Subscriptions.Queries.GetSubscriptionById;

namespace PrimeFit.API.Controllers
{
    public class SubscriptionsController : BaseController
    {

        [HttpGet("{subscriptionId:int}", Name = RouteNames.Subscriptions.GetSubscriptionById)]
        public async Task<IActionResult> GetByiD([FromRoute] int subscriptionId)
        {
            var query = new GetSubscriptionByIdQuery { SubscriptionId = subscriptionId };
            var result = await Mediator.Send(query);
            if (result.IsError)
            {
                return Problem(result.Errors);
            }
            return Ok(result.Value);
        }


        [HttpPatch("{id:int}/freeze")]
        public async Task<IActionResult> Freeze([FromRoute] int Id)
        {
            var command = new FreezeSubscriptionCommand { SubscriptionId = Id };

            var result = await Mediator.Send(command);
            if (result.IsError)
            {
                return Problem(result.Errors);
            }

            return NoContent();
        }

        [HttpPatch("{id:int}/unfreeze")]
        public async Task<IActionResult> Unfreeze([FromRoute] int Id)
        {
            var command = new UnfreezeSubscriptionCommand { SubscriptionId = Id };

            var result = await Mediator.Send(command);
            if (result.IsError)
            {
                return Problem(result.Errors);
            }

            return NoContent();
        }


    }
}
