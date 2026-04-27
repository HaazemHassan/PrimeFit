namespace PrimeFit.Application.Features.Branches.Caching
{
    public static class BranchesCache
    {

        public static string ListTag()
             => "tag:branch:list";

        public static string ByIdTag(int branchId)
            => $"tag:branch:{branchId}";

        public static string OwnerTag(int ownerId)
             => $"tag:branch:owner:{ownerId}";

        public static string ByIdCacheKey(int branchId)
            => $"branch:{branchId}:details:admin";

        public static string ByIdForPublicCacheKey(int branchId)
            => $"branch:{branchId}:details:public";

        public static string SetupDetailsCacheKey(int branchId)
            => $"branch:{branchId}:setup";

        public static string PublicPaginatedCacheKey(int pageNumber, int pageSize)
            => $"branch:pageNumber:{pageNumber}:size:{pageSize}";

        public static string OwnerPaginatedCacheKey(int ownerId, int pageNumber, int pageSize)
            => $"branch:owner:{ownerId}:pageNumber:{pageNumber}:size:{pageSize}";

    }
}
