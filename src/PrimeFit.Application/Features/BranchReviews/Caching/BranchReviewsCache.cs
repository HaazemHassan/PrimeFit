namespace PrimeFit.Application.Features.BranchReviews.Caching
{
    public static class BranchReviewsCache
    {

        public static string ListTag(int branchId)
            => $"tag:branch:{branchId}:reviews:list";

        public static string PaginatedCacheKey(int branchId, int pageNumber, int pageSize)
            => $"branch:{branchId}:reviews:pageNumber:{pageNumber}:size:{pageSize}";

    }
}
