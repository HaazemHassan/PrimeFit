using Ardalis.Specification;
using PrimeFit.Domain.Common.Enums;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Specifications.Branches
{
    public class BranchWithWorkingHoursSpec : Specification<Branch>
    {
        public BranchWithWorkingHoursSpec(int branchId, BranchStatus? status = default)
        {
            Query.Where(b => b.Id == branchId);

            if (status.HasValue)
            {
                Query.Where(b => b.BranchStatus == status.Value);
            }

            Query.Include(b => b.WorkingHours);
        }
    }
}
