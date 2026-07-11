using Ardalis.Specification;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Specifications.Employees
{
    public class BranchEmployeesPaginatedSpec : BranchEmployeesFilteredSpec
    {
        public BranchEmployeesPaginatedSpec(int branchId, int pageNumber, int pageSize, string? search = null)
            : base(branchId, search)
        {
            Query.OrderBy(e => e.CreatedAt)
                 .Paginate(pageNumber, pageSize)
                 .AsNoTracking();
        }
    }
}
