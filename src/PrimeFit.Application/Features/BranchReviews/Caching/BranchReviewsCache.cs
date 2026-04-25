namespace PrimeFit.Application.Features.BranchReviews.Caching
{
    public static class BranchReviewsCache
    {

        public static string Tag(int branchId)
            => $"branch:{branchId}:reviews";

        public static string Paginated(int branchId, int pageNumber, int pageSize)
            => $"branch:{branchId}:reviews:pageNumber:{pageNumber}:size:{pageSize}";

    }
}
