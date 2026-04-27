namespace PrimeFit.Application.Security.Caching
{
    public static class SecurityCache
    {
        public static string BranchAuthTag(int branchId)
             => $"auth:branch:{branchId}";

        public static string BranchAuthKey(int userId, int branchId)
             => $"auth:branch:{branchId}:user:{userId}";
    }
}
