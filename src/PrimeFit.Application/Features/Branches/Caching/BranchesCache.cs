namespace PrimeFit.Application.Features.Branches.Caching
{
    public static class BranchesCache
    {
        public static string ListTag()
             => "tag:branch:list";

        public static string Tag(int branchId)
            => $"tag:branch:{branchId}";

        public static string ById(int branchId)
            => $"branch:{branchId}:details";

        public static string SetupDetails(int branchId)
            => $"branch:{branchId}:setup";

        public static string BranchEmployees(int branchId, int pageNumber, int pageSize)
            => $"branch:{branchId}:employees:page:{pageNumber}:size:{pageSize}";

        public static string BranchPackages(int branchId, int pageNumber, int pageSize)
            => $"branch:{branchId}:packages:page:{pageNumber}:size:{pageSize}";

        public static string Paginated(int pageNumber, int pageSize)
            => $"branch:pageNumber:{pageNumber}:size:{pageSize}";

        public static string OwnerTag(int ownerId)
            => $"branch:owner:{ownerId}";

        public static string OwnerPaginated(int ownerId, int pageNumber, int pageSize)
            => $"branch:owner:{ownerId}:pageNumber:{pageNumber}:size:{pageSize}";
    }
}
