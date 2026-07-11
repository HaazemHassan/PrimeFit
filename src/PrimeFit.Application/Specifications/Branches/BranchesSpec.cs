using Ardalis.Specification;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Specifications.Branches
{
    public class BranchesSpec : BranchesFilteredSpec
    {
        public BranchesSpec(int? ownerId, int? pageNumber, int? pageSize, string? search)
            : base(ownerId, search)
        {
            Query.Include(b => b.Governorate);

            Query.OrderByDescending(b => b.CreatedAt);

            Query.Paginate(pageNumber, pageSize);

            Query.AsNoTracking();

        }
    }
}