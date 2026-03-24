using Ardalis.Specification;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Specifications.Employees
{
    public class BranchEmployeesPaginatedSpec : Specification<Employee>
    {
        public BranchEmployeesPaginatedSpec(int branchId, int pageNumber, int pageSize)
        {
            Query.Where(e => e.BranchId == branchId);

            Query.OrderBy(e => e.CreatedAt)
                 .Paginate(pageNumber, pageSize)
                 .AsNoTracking();
        }
    }
}
