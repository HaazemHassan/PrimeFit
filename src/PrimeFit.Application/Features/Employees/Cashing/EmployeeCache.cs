namespace PrimeFit.Application.Features.Employees.Cashing
{
    public static class EmployeeCache
    {
        public static string EmployeeRolesCacheKey()
           => "lookups:employees:roles";

        public static string BranchEmployeesPaginatedCacheKey(int branchId, int pageNumber, int pageSize)
           => $"branch:{branchId}:employees:page:{pageNumber}:size:{pageSize}";
    }
}
