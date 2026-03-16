using ErrorOr;
using MediatR;
using PrimeFit.Application.Features.Branches.Shared.DTOS;

namespace PrimeFit.Application.Features.Governorates.Queries.GetGovernorates
{
    public class GetGovernoratesQuery : IRequest<ErrorOr<List<GovernorateDto>>>
    {
    }
}
