using Ardalis.Specification;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Specifications.Branches
{
    public class BranchesPaginatedForOwnerSpec : Specification<Branch>
    {
        public BranchesPaginatedForOwnerSpec(int? ownerId, int pageNumber, int pageSize, string? search, string? sortBy)
        {
            if (ownerId.HasValue)
                Query.Where(b => b.OwnerId == ownerId);

            if (!string.IsNullOrEmpty(search))
            {
                Query.Where(u => u.Name.Contains(search));
            }

            Query.Include(b => b.Governorate);

            if (sortBy == "name_desc")
                Query.OrderByDescending(u => u.Name);
            else if (sortBy == "name_asc")
                Query.OrderBy(b => b.Name);
            else
                Query.OrderByDescending(b => b.CreatedAt);

            Query.Paginate(pageNumber, pageSize);

            Query.AsNoTracking();

        }
    }
}