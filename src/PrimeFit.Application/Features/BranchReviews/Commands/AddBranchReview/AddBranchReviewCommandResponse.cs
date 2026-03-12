namespace PrimeFit.Application.Features.BranchReviews.Commands.AddBranchReview
{
    public class AddBranchReviewCommandResponse
    {
        public int Id { get; set; }
        public int BranchId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTimeOffset ReviewedAt { get; set; }
    }
}
