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
using PrimeFit.Application.Features.Governorates.Queries.GetGovernorates;

namespace PrimeFit.API.Controllers
{
    public class GovernoratesController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetGovernorates()
        {
            var result = await Mediator.Send(new GetGovernoratesQuery());
            if (result.IsError)
            {
                return Problem(result.Errors);
            }

            return Ok(result.Value);
        }
    }
}
