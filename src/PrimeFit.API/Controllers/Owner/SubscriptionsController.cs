using Microsoft.AspNetCore.Mvc;
using PrimeFit.API.Common.Constants;
using PrimeFit.Application.Features.Subscriptions.Commands.AddSubscription;
using PrimeFit.Application.Features.Subscriptions.Commands.CancelSubscription;

namespace PrimeFit.API.Controllers.Owner
{
    public class SubscriptionsController : OwnerBaseController
    {

        [HttpPost()]
        public async Task<IActionResult> Add([FromBody] AddSubscriptionCommand command)
        {
            var result = await Mediator.Send(command);
            if (result.IsError)
            {
                return Problem(result.Errors);
            }


            //i will update this lateer
            return CreatedAtRoute(RouteNames.Branches.GetBranchById, new { id = result.Value.Id }, result.Value);
        }



        [HttpPatch("{id:int}/cancel")]
        public async Task<IActionResult> Cancel([FromRoute] int Id)
        {
            var command = new CancelSubscriptionCommand { SubscriptionId = Id };

            var result = await Mediator.Send(command);
            if (result.IsError)
            {
                return Problem(result.Errors);
            }

            return NoContent();
        }



    }
}
