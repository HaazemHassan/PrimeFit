using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrimeFit.API.Common.Constants;
using PrimeFit.Application.Features.Packages.Queries.GetBranchPackagesForUsers;
using PrimeFit.Application.Features.Users.Queries.GetUsersPaginated;

namespace PrimeFit.API.Controllers
{
    public class BranchesController : BaseController
    {


        [HttpGet("{Id:int}", Name = RouteNames.Branches.GetBranchById)]
        public async Task<IActionResult> GetBranchById([FromRoute] int Id)
        {
            return NoContent();
        }


        [HttpGet()]
        [Authorize]
        public async Task<IActionResult> GetBranches([FromQuery] GetMyBranchesQuery query)
        {
            var result = await Mediator.Send(query);
            if (result.IsError)
            {
                return Problem(result.Errors);
            }

            return Ok(result.Value);
        }


        [HttpGet("{branchId:int}/packages")]
        public async Task<IActionResult> GetBranchPackages([FromRoute] int branchId)
        {
            var query = new GetBranchPackagesForUsersQuery(branchId);

            var result = await Mediator.Send(query);
            if (result.IsError)
            {
                return Problem(result.Errors);
            }
            return Ok(result.Value);
        }



    }
}
