using AutoMapper;
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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrimeFit.API.Common.Constants;
using PrimeFit.Application.Contracts.Api;
using PrimeFit.Application.Features.Subscriptions.Queries.GetMySubscriptionById;
using PrimeFit.Application.Features.Subscriptions.Queries.GetMySubscriptions;
using PrimeFit.Application.Features.Users.Commands.UpdateProfile;
using PrimeFit.Application.Features.Users.Queries.CheckEmailAvailability;
using PrimeFit.Application.Features.Users.Queries.GetMe;
using PrimeFit.Application.Features.Users.Queries.GetUserById;
using PrimeFit.Domain.Common.Enums;

namespace PrimeFit.API.Controllers
{
    public class UsersController(ICurrentUserService _currentUserService, IMapper _mapper) : BaseController
    {


        [HttpGet("{Id:int}", Name = RouteNames.Users.GetUserById)]
        public async Task<IActionResult> GetById([FromRoute] int Id)
        {
            var query = new GetUserByIdQuery(Id);
            var result = await Mediator.Send(query);
            if (result.IsError)
                return Problem(result.Errors);
            return Ok(result.Value);
        }



        [HttpGet("check-email")]
        public async Task<IActionResult> CheckEmailAvailability([FromQuery] CheckEmailAvailabilityQuery query)
        {
            var result = await Mediator.Send(query);
            if (result.IsError)
                return Problem(result.Errors);
            return Ok(result.Value);
        }



        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetMe()
        {
            var result = await Mediator.Send(new GetMeQuery());
            if (result.IsError)
            {
                return Problem(result.Errors);
            }

            return Ok(result.Value);
        }



        [HttpPatch("me")]
        [Authorize]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest request)
        {
            var command = _mapper.Map<UpdateProfileCommand>(request);
            command.UserId = _currentUserService.UserId!.Value;

            var result = await Mediator.Send(command);

            if (result.IsError)
            {
                return Problem(result.Errors);

            }

            return Ok(result.Value);
        }


        [HttpGet("me/subscriptions")]
        [Authorize]
        public async Task<IActionResult> GetMySubscriptions([FromQuery] SubscriptionStatus? status)
        {
            var query = new GetMySubscriptionsQuery { Status = status };
            var result = await Mediator.Send(query);
            if (result.IsError)
            {
                return Problem(result.Errors);
            }
            return Ok(result.Value);
        }

        [HttpGet("me/subscriptions/{id:int}")]
        [Authorize]
        public async Task<IActionResult> GetMySubscriptionById([FromRoute] int id)
        {
            var query = new GetMySubscriptionByIdQuery { SubscriptionId = id };
            var result = await Mediator.Send(query);
            if (result.IsError)
            {
                return Problem(result.Errors);
            }
            return Ok(result.Value);
        }

    }
}

