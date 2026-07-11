using Ardalis.Specification;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Specifications.BranchPackages
{
    public class ActivePackagesPaginatedForBranchSpec : BranchPackagesFilteredSpec
    {
        public ActivePackagesPaginatedForBranchSpec(int branchId, int pageNumber, int pageSize, string? search = null)
            : base(branchId, search, isActive: true)
        {
            Query.OrderBy(p => p.CreatedAt);
            Query.Paginate(pageNumber, pageSize);
            Query.AsNoTracking();
        }
    }
}
