using Ardalis.Specification;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Specifications.BranchPackages
{
    public class BranchPackagesPaginatedSpec : BranchPackagesFilteredSpec
    {
        public BranchPackagesPaginatedSpec(int branchId, int pageNumber, int pageSize, string? search = null)
            : base(branchId, search, null)
        {
            Query.OrderBy(p => p.CreatedAt).ThenBy(p => p.IsActive);
            Query.Paginate(pageNumber, pageSize);
            Query.AsNoTracking();
        }
    }
}
