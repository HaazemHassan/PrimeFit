using ErrorOr;
using MediatR;
using PrimeFit.Application.Security;
using PrimeFit.Application.Security.Markers;

namespace PrimeFit.Application.Features.BranchReviews.Commands.UpdateMyBranchReview
{
    [Authorize]
    public class UpdateMyBranchReviewCommand : IRequest<ErrorOr<UpdateMyBranchReviewCommandResponse>>, IAuthorizedRequest
    {
        public int BranchId { get; set; }
        public int ReviewId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
    }
}
