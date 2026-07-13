using Microsoft.AspNetCore.Authorization;
using PrimeFit.Api.Requests.Branches.AddBranchImage;
using PrimeFit.Api.Requests.Branches.UpdateBranchImage;
using PrimeFit.Api.Requests.Branches.GetBranchById;
using PrimeFit.Api.Requests.Branches.ActivateBranchImages;
using PrimeFit.Api.Requests.Branches.GetBranchStatistics;
using PrimeFit.Api.Requests.Branches.GetOwnerBranchesStatistics;
using PrimeFit.Api.Requests.Branches.UpdateBranchStatus;
using PrimeFit.Api.Requests.Branches.UpdateBasicDetails;
using PrimeFit.Api.Requests.Branches.UpdateLocationDetailsRequest;
using PrimeFit.Api.Requests.Branches.UpdateWorkingHoursRequest;
using PrimeFit.Api.Requests.BranchPackages.AddPackage;
using PrimeFit.Api.Requests.BranchPackages.UpdatePackage;
using PrimeFit.Api.Requests.BranchPackages.UpdatePackageStatus;
using PrimeFit.Api.Requests.BranchPackages.GetBranchPackagesForCustomers;
using PrimeFit.Api.Requests.Employees.GetBranchEmployees;
using PrimeFit.Api.Requests.Employees.UpdateEmployeeRequest;
using PrimeFit.Api.Requests.Subscriptions.AddSubscription;
using PrimeFit.Api.Requests.Subscriptions.CreateMemberWithSubscription;
using PrimeFit.Api.Requests.Subscriptions.GetBranchSubscriptions;
using PrimeFit.Api.Requests.Subscriptions.GetSubscriptionAttendanceHistory;
using PrimeFit.Api.Requests.Common.Pagination;
using PrimeFit.Api.Requests.Users.UpdateProfile;
using PrimeFit.Api.Requests.BranchReviews;
using PrimeFit.Api.Requests.BranchReviews.GetBranchReviewsRequest;
using PrimeFit.Api.Requests.Notifications;
using PrimeFit.Api.Requests.Payments;
using Microsoft.AspNetCore.Mvc;
using PrimeFit.API.Common.Constants;
using PrimeFit.Application.Contracts.Api;
using PrimeFit.Application.Features.Branches.Queries.GetBranchById;
using PrimeFit.Application.Features.Branches.Queries.GetBranchByIdForPublic;
using PrimeFit.Application.Features.Branches.Queries.GetBranchesForPublic;
using PrimeFit.Application.Features.Branches.Queries.GetMyBranches;
using PrimeFit.Application.Features.BranchPackages.Queries.GetBranchPackagesForCustomers;
using PrimeFit.Domain.Common.Enums;
using AutoMapper;
using PrimeFit.Api.Requests.Branches.GetMyBranches;
using PrimeFit.Api.Requests.Branches.GetBranchesForPublic;

namespace PrimeFit.API.Controllers
{
    public class BranchesController : BaseController
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public BranchesController(ICurrentUserService currentUserService, IMapper mapper)
        {
            _currentUserService = currentUserService;
            _mapper = mapper;
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
        public async Task<IActionResult> GetBranches([FromQuery] GetMyBranchesRequest request)
        {
            var query = _mapper.Map<GetMyBranchesQuery>(request);
            var result = await Mediator.Send(query);
            if (result.IsError)
            {
                return Problem(result.Errors);
            }

            return Ok(result.Value);
        }


        [HttpGet("{branchId:int}/packages")]
        public async Task<IActionResult> GetBranchPackages([FromRoute] int branchId, [FromQuery] GetBranchPackagesForCustomersRequest request)
        {
            var query = new GetBranchPackagesForCustomersQuery(branchId, request.PageNumber, request.PageSize);

            var result = await Mediator.Send(query);
            if (result.IsError)
            {
                return Problem(result.Errors);
            }
            return Ok(result.Value);
        }

        [HttpGet("public")]
        public async Task<IActionResult> GetBranches([FromQuery] GetBranchesForPublicRequest request)
        {
            request.RadiusInMeters = 100000;
            var query = _mapper.Map<GetBranchesForPublicQuery>(request);
            var result = await Mediator.Send(query);
            if (result.IsError)
            {
                return Problem(result.Errors);
            }
            return Ok(result.Value);
        }

    }
}
