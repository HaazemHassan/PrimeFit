using ErrorOr;
using MediatR;
using PrimeFit.Application.Security;
using PrimeFit.Application.Security.Markers;

namespace PrimeFit.Application.Features.BranchReviews.Commands.AddBranchReview
{
    [Authorize]
    public class AddBranchReviewCommand : IRequest<ErrorOr<AddBranchReviewCommandResponse>>, IAuthorizedRequest
    {
        public int BranchId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
    }
}
