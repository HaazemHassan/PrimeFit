using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrimeFit.API.Common.Constants;
using PrimeFit.API.Requests.Branches;
using PrimeFit.Application.Contracts.Api;
using PrimeFit.Application.Features.Branches.Queries.GetBranchById;
using PrimeFit.Application.Features.Branches.Queries.GetBranchByIdForPublic;
using PrimeFit.Application.Features.Branches.Queries.GetBranchesForPublic;
using PrimeFit.Application.Features.Branches.Queries.GetMyBranches;
using PrimeFit.Application.Features.BranchPackages.Queries.GetBranchPackagesForCustomers;
using PrimeFit.Domain.Common.Enums;

namespace PrimeFit.API.Controllers
{
    public class BranchesController : BaseController
    {
        private readonly ICurrentUserService _currentUserService;

        public BranchesController(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
        }

        [HttpGet("{branchId:int}", Name = RouteNames.Branches.GetBranchById)]
        public async Task<IActionResult> GetBranchById([FromRoute] int branchId, [FromQuery] GetBranchByIdRequest request)
        {
            if (!_currentUserService.IsAuthenticated || _currentUserService.UserType == UserType.Customer)
            {
                var branchForPublicQuery = new GetBranchByIdForPublicQuery(branchId, request.Latitude, request.Longitude);

                var publicResult = await Mediator.Send(branchForPublicQuery);

                if (publicResult.IsError)
                {
                    return Problem(publicResult.Errors);

                }
                return Ok(publicResult.Value);
            }

            var branchForAdminQuery = new GetBranchByIdQuery(branchId);

            var ownerResult = await Mediator.Send(branchForAdminQuery);

            if (ownerResult.IsError)
            {
                return Problem(ownerResult.Errors);

            }

            return Ok(ownerResult.Value);
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
            var query = new GetBranchPackagesForCustomersQuery(branchId);

            var result = await Mediator.Send(query);
            if (result.IsError)
            {
                return Problem(result.Errors);
            }
            return Ok(result.Value);
        }

        [HttpGet("public")]
        public async Task<IActionResult> GetBranches([FromQuery] GetBranchesForPublicQuery query)
        {
            var result = await Mediator.Send(query);
            if (result.IsError)
            {
                return Problem(result.Errors);
            }
            return Ok(result.Value);
        }

    }
}
