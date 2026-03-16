using Ardalis.Specification;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Features.Governorates.Queries.GetGovernorates
{
    public class GetGovernoratesSpec : Specification<Governorate>
    {
        public GetGovernoratesSpec()
        {
            Query.OrderBy(g => g.Name);
            Query.AsNoTracking();
        }
    }
}
