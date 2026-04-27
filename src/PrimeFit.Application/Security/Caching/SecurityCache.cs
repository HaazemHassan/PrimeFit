namespace PrimeFit.Application.Security.Caching
{
    public static class SecurityCache
    {
        public static string BranchAuthTag(int branchId)
             => $"tag:auth:branch:{branchId}";

        public static string BranchAuthCacheKey(int userId, int branchId)
             => $"auth:branch:{branchId}:user:{userId}";
    }
}
