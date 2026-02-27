using Ardalis.Specification;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Specifications.Branches
{
    public class BranchWithBasicDetailsSpec : Specification<Branch>
    {

        public BranchWithBasicDetailsSpec(int branchId)
        {
            Query.Where(b => b.Id == branchId);


            Query.Include(b => b.WorkingHours);
            Query.Include(b => b.Images);
        }
    }
}
