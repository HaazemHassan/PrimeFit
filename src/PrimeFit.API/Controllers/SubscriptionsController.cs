using Microsoft.AspNetCore.Mvc;
using PrimeFit.API.Common.Constants;
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


    }
}
