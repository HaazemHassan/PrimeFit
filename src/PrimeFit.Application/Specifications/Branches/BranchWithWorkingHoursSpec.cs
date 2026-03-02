using Ardalis.Specification;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Specifications.Branches
{
    public class BranchWithWorkingHoursSpec : Specification<Branch>
    {
        public BranchWithWorkingHoursSpec(int branchId)
        {
            Query.Where(b => b.Id == branchId)
                 .Include(b => b.WorkingHours);
        }
    }
}
