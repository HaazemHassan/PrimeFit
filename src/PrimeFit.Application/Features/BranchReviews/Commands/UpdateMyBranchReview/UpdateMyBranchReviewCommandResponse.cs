namespace PrimeFit.Application.Features.BranchReviews.Commands.UpdateMyBranchReview
{
    public class UpdateMyBranchReviewCommandResponse
    {
        public int Id { get; set; }
        public int BranchId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTimeOffset ReviewedAt { get; set; }
    }
}
