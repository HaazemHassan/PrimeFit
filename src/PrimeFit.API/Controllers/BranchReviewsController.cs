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
using PrimeFit.Api.Requests.BranchReviews.GetBranchReviewsRequest;
using PrimeFit.Api.Requests.BranchReviews.AddBranchReview;
using PrimeFit.Api.Requests.Notifications;
using PrimeFit.Api.Requests.Payments;
using Microsoft.AspNetCore.Mvc;
using PrimeFit.Application.Features.BranchReviews.Commands.AddBranchReview;
using PrimeFit.Application.Features.BranchReviews.Commands.UpdateMyBranchReview;
using PrimeFit.Application.Features.BranchReviews.Queries.GetBranchReviews;
using PrimeFit.API.Requests.BranchReviews.UpdateMyBranchReviewRequest;

namespace PrimeFit.API.Controllers
{
    [Route("api/branches/{branchId}/reviews")]
    public class BranchReviewsController : BaseController
    {
        private readonly IMapper _mapper;

        public BranchReviewsController(IMapper mapper)
        {
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetReviews([FromRoute] int branchId, [FromQuery] GetBranchReviewsRequest request)
        {
            var query = _mapper.Map<GetBranchReviewsQuery>(request);
            query.BranchId = branchId;

            var result = await Mediator.Send(query);
            if (result.IsError)
            {
                return Problem(result.Errors);
            }
            return Ok(result.Value);
        }

        [HttpPost]
        public async Task<IActionResult> AddReview([FromRoute] int branchId, [FromBody] AddBranchReviewRequest request)
        {
            var command = _mapper.Map<AddBranchReviewCommand>(request);
            command.BranchId = branchId;

            var result = await Mediator.Send(command);
            if (result.IsError)
            {
                return Problem(result.Errors);
            }
            return Created(string.Empty, result.Value);
        }


        [HttpPut("{reviewId:int}")]
        public async Task<IActionResult> UpdateMyReview([FromRoute] int branchId, [FromRoute] int reviewId, [FromBody] UpdateMyBranchReviewRequest request)
        {
            var command = new UpdateMyBranchReviewCommand
            {
                BranchId = branchId,
                ReviewId = reviewId,
                Rating = request.Rating,
                Comment = request.Comment
            };

            var result = await Mediator.Send(command);
            if (result.IsError)
            {
                return Problem(result.Errors);
            }
            return Ok(result.Value);
        }

    }
}
