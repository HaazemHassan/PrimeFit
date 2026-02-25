using Ardalis.Specification;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Specifications.Branches
{
    public class BranchForOwnerSpec : Specification<Branch>
    {
        public BranchForOwnerSpec(int branchId, int ownerId)
        {

            Query.Where(b => b.OwnerId == ownerId && b.Id == branchId);
        }
    }
}
