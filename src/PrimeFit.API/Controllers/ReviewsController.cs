using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PrimeFit.API.Requests.BranchReviews;
using PrimeFit.Application.Features.BranchReviews.Commands.UpdateMyBranchReview;

namespace PrimeFit.API.Controllers
{
    public class ReviewsController : BaseController
    {
        private readonly IMapper _mapper;

        public ReviewsController(IMapper mapper)
        {
            _mapper = mapper;
        }


        [HttpPut("{reviewId:int}")]
        public async Task<IActionResult> UpdateMyReview([FromRoute] int reviewId, [FromBody] UpdateMyBranchReviewRequest request)
        {
            var command = new UpdateMyBranchReviewCommand
            {
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
