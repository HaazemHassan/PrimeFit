using Ardalis.Specification;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Specifications.Employees
{
    public class EmployeeWithPermissionsByUserIdAndBranchIdSpec : Specification<Employee>, ISingleResultSpecification<Employee>
    {
        public EmployeeWithPermissionsByUserIdAndBranchIdSpec(int userId, int branchId)
        {
            Query.Where(e => e.UserId == userId && e.BranchId == branchId && e.Active)
                 .Include(e => e.Role)
                 .ThenInclude(r => r.Permissions);
        }
    }
}
