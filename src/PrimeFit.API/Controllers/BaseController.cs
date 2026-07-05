using ErrorOr;
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
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace PrimeFit.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableRateLimiting("defaultLimiter")]
    public class BaseController : ControllerBase
    {

        private IMediator? _mediatorInstance;
        protected IMediator Mediator => _mediatorInstance ??= HttpContext.RequestServices.GetService<IMediator>()!;

        #region Actions

        protected ActionResult Problem(List<Error> errors)
        {
            var problem = Results.Problem(statusCode: GetStatusCode(errors.FirstOrDefault().Type));
            var problemDetail = problem.GetType().GetProperty(nameof(ProblemDetails))!.GetValue(problem) as ProblemDetails;
            problemDetail!.Extensions = new Dictionary<string, object?>()
            {
                ["errors"] = errors.Select(e => new
                {
                    e.Code,
                    e.Description,
                    Field = e.Metadata != null && e.Metadata.TryGetValue("field", out object? value) ? value : null
                }),
                ["traceId"] = HttpContext.TraceIdentifier
            };


            return new ObjectResult(problemDetail);


        }

        private static int GetStatusCode(ErrorType errorType)
        {
            return errorType switch
            {
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
                ErrorType.Forbidden => StatusCodes.Status403Forbidden,
                _ => StatusCodes.Status500InternalServerError,
            };
        }

        #endregion


    }

}


