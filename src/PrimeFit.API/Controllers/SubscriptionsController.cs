using Microsoft.AspNetCore.Mvc;
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
using PrimeFit.API.Common.Constants;
using PrimeFit.Application.Features.Subscriptions.Commands.FreezeSubscription;
using PrimeFit.Application.Features.Subscriptions.Commands.UnfreezeSubscription;
using PrimeFit.Application.Features.Subscriptions.Queries.GetSubscriptionAttendanceHistory;
using PrimeFit.Application.Features.Subscriptions.Queries.GetSubscriptionById;
using AutoMapper;

namespace PrimeFit.API.Controllers
{
    public class SubscriptionsController(IMapper _mapper) : BaseController
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

        [HttpGet("{subscriptionId:int}/attendance-history")]
        public async Task<IActionResult> GetAttendanceHistory([FromRoute] int subscriptionId, [FromQuery] GetSubscriptionAttendanceHistoryRequest request)
        {
            var query = _mapper.Map<GetSubscriptionAttendanceHistoryQuery>(request);
            query.SubscriptionId = subscriptionId;

            var result = await Mediator.Send(query);
            if (result.IsError)
            {
                return Problem(result.Errors);
            }

            return Ok(result.Value);
        }

    }
}

