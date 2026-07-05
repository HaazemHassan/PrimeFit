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
