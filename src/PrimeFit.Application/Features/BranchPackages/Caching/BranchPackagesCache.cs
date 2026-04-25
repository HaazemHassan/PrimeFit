namespace PrimeFit.Application.Features.BranchPackages.Caching
{
    public static class BranchPackagesCache
    {
        public static string Tag(int branchId) => $"branch:{branchId}:packages";


        public static string Paginated(int branchId, int pageNumber, int pageSize)
           => $"branch:{branchId}:packages:pageNumber:{pageNumber}:size:{pageSize}";

    }
}
