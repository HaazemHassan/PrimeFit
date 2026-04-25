using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PrimeFit.API.Requests.BranchReviews;
using PrimeFit.Application.Features.BranchReviews.Commands.AddBranchReview;
using PrimeFit.Application.Features.BranchReviews.Commands.UpdateMyBranchReview;
using PrimeFit.Application.Features.BranchReviews.Queries.GetBranchReviews;

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
        public async Task<IActionResult> AddReview([FromRoute] int branchId, [FromBody] AddBranchReviewCommand command)
        {
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
