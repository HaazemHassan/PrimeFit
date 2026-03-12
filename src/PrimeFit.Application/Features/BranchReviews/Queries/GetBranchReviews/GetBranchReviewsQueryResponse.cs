namespace PrimeFit.Application.Features.BranchReviews.Queries.GetBranchReviews
{
    public class GetBranchReviewsQueryResponse
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserFullName { get; set; } = null!;
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTimeOffset ReviewedAt { get; set; }
    }

    public record BranchReviewsMeta(GetBranchReviewsQueryResponse? MyReview);
}
