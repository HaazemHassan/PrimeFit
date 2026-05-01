using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PrimeFit.Application.Features.Subscriptions.Commands.CancelSubscription;

namespace PrimeFit.API.Controllers.Owner
{
    public class SubscriptionsController : OwnerBaseController
    {
        private readonly IMapper _mapper;

        public SubscriptionsController(IMapper mapper)
        {
            _mapper = mapper;
        }

        //[HttpPost()]
        //public async Task<IActionResult> Add([FromBody] AddSubscriptionRequest request, [FromHeader(Name = "x-request-id")] string requestId)
        //{
        //    var command = new AddSubscriptionCommand(Guid.Parse(requestId))
        //    {
        //        Email = request.Email,
        //        PackageId = request.PackageId,
        //        BranchId = request.BranchId

        //    };
        //    var result = await Mediator.Send(command);
        //    if (result.IsError)
        //    {
        //        return Problem(result.Errors);
        //    }


        //    return CreatedAtRoute(RouteNames.Subscriptions.GetSubscriptionById, new { id = result.Value.Id }, result.Value);
        //}



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
