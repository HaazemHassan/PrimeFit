using Ardalis.Specification;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Specifications.Users
{
    public class UsersFilterPaginatedSpec : Specification<DomainUser>
    {
        public UsersFilterPaginatedSpec(int pageNumber, int pageSize, string? search, string? sortBy)
        {
            if (!string.IsNullOrEmpty(search))
            {
                Query.Where(u => u.FirstName.Contains(search) || u.Email.Contains(search));
            }

            if (sortBy == "name_desc")
                Query.OrderByDescending(u => u.FirstName);
            else
                Query.OrderBy(u => u.FirstName);

            Query.Paginate(pageNumber, pageSize);

            Query.AsNoTracking();

        }
    }
}