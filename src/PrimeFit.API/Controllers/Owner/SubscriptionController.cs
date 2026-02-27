using Microsoft.AspNetCore.Mvc;
using PrimeFit.API.Common.Constants;
using PrimeFit.Application.Features.Subscriptions.Commands.AddSubscription;

namespace PrimeFit.API.Controllers.Owner
{
    public class SubscriptionController : OwnerBaseController
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
    }
}
