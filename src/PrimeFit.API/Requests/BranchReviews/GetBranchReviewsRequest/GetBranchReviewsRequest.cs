namespace PrimeFit.Api.Requests.BranchReviews.GetBranchReviewsRequest {
    public class GetBranchReviewsRequest
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? Search { get; set; }
        public int? Rating { get; set; }
    }
}
