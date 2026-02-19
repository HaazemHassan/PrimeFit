using Ardalis.Specification;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Domain.Specifications.Users
{
    public class UsersSearchSpec : Specification<DomainUser>
    {
        public UsersSearchSpec(string? search)
        {
            if (!string.IsNullOrEmpty(search))
            {
                Query.Where(u => u.FirstName.Contains(search) || u.LastName.Contains(search) || u.Email.Contains(search));
            }
        }
    }
}
