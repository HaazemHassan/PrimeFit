namespace PrimeFit.Application.Features.Branches.Caching
{
    public static class BranchesCache
    {
        public static string Tag()
             => "branch";

        public static string Tag(int branchId)
            => $"branch:{branchId}";


        public static string ById(int branchId)
            => $"branch:{branchId}";

        public static string Paginated(int pageNumber, int pageSize)
            => $"branch:pageNumber:{pageNumber}:size:{pageSize}";

    }
}
