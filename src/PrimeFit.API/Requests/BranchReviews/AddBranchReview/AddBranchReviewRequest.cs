using ErrorOr;

namespace PrimeFit.Api.Requests.BranchReviews.AddBranchReview;

public class AddBranchReviewRequest
{
        public int BranchId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
}
