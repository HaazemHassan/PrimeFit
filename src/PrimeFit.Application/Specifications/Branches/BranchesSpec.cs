using Ardalis.Specification;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Specifications.Branches
{
    public class BranchesSpec : Specification<Branch>
    {
        public BranchesSpec(int? ownerId, int? pageNumber, int? pageSize, string? search)
        {
            if (ownerId.HasValue)
            {
                Query.Where(b => b.OwnerId == ownerId);
            }

            if (!string.IsNullOrEmpty(search))
            {
                Query.Where(u => u.Name.Contains(search));
            }

            Query.Include(b => b.Governorate);


            Query.OrderByDescending(b => b.CreatedAt);

            Query.Paginate(pageNumber, pageSize);

            Query.AsNoTracking();

        }
    }
}