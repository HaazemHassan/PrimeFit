
using Ardalis.Specification;
using PrimeFit.Domain.Entities.Base;

namespace PrimeFit.Application.Specifications.Shared
{
    public class GetByIdSpec<T> : Specification<T> where T : BaseEntity<int>
    {
        public GetByIdSpec(int userId)
        {
            Query.Where(u => u.Id == userId);

        }
    }
}