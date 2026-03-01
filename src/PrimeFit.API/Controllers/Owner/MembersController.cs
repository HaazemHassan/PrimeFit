using Microsoft.AspNetCore.Mvc;
using PrimeFit.API.Common.Constants;
using PrimeFit.Application.Features.Members.Commands.CreateMemberWithSubscription;

namespace PrimeFit.API.Controllers.Owner
{
    public class MembersController : OwnerBaseController
    {


        [HttpPost()]
        public async Task<IActionResult> CreateMemberWithSubscription([FromBody] CreateMemberWithSubscriptionCommand command)
        {
            var result = await Mediator.Send(command);
            if (result.IsError)
            {
                return Problem(result.Errors);
            }


            //i will update this later
            return CreatedAtRoute(RouteNames.Branches.GetBranchById, new { id = result.Value.Id }, result.Value);
        }
    }
}
