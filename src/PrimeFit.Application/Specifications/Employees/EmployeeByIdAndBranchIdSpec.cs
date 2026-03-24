using Ardalis.Specification;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Specifications.Employees
{
    public class EmployeeByIdAndBranchIdSpec : Specification<Employee>, ISingleResultSpecification<Employee>
    {
        public EmployeeByIdAndBranchIdSpec(int employeeId, int branchId)
        {
            Query.Where(e => e.Id == employeeId && e.BranchId == branchId)
                .Include(e => e.User).Include(e => e.Role);
        }
    }
}
