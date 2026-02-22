using Ardalis.Specification;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Specifications.Branches
{
    public class BranchesPaginatedSpec : Specification<Branch>
    {
        public BranchesPaginatedSpec(int? ownerId, int pageNumber, int pageSize, string? search, string? sortBy)
        {
            if (ownerId.HasValue)
                Query.Where(u => u.OwnerId == ownerId);

            if (!string.IsNullOrEmpty(search))
            {
                Query.Where(u => u.Name.Contains(search));
            }

            if (sortBy == "name_desc")
                Query.OrderByDescending(u => u.Name);
            else
                Query.OrderBy(u => u.Name);

            Query.Paginate(pageNumber, pageSize);

            Query.AsNoTracking();

        }
    }
}