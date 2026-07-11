using Ardalis.Specification;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Specifications.Employees
{
    public class BranchEmployeesFilteredSpec : Specification<Employee>
    {
        public BranchEmployeesFilteredSpec(int branchId, string? search = null)
        {
            Query.Where(e => e.BranchId == branchId);

            if (!string.IsNullOrEmpty(search))
            {
                var lowerSearch = search.ToLower();
                Query.Where(e => e.User.FirstName.ToLower().Contains(lowerSearch) ||
                                 e.User.LastName.ToLower().Contains(lowerSearch) ||
                                 e.User.Email.ToLower().Contains(lowerSearch));
            }
        }
    }
}
