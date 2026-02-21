using Ardalis.Specification;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Domain.Specifications.Branches
{
    public class GetBranchWithWorkingHoursSpec : Specification<Branch>
    {
        public GetBranchWithWorkingHoursSpec(int branchId)
        {
            Query.Where(x => x.Id == branchId)
                .Include(x => x.WorkingHours);
        }
    }
}
