namespace PrimeFit.Application.Features.BranchPackages.Caching
{
    public static class BranchPackagesCache
    {
        public static string ListTag(int branchId) => $"tag:branch:{branchId}:packages:list";


        public static string AdminPaginatedCacheKey(int branchId, int pageNumber, int pageSize)
           => $"branch:{branchId}:packages:admin:page:{pageNumber}:size:{pageSize}";

        public static string PublicPaginatedCacheKey(int branchId, int pageNumber, int pageSize)
           => $"branch:{branchId}:packages:public:page:{pageNumber}:size:{pageSize}";

    }
}
