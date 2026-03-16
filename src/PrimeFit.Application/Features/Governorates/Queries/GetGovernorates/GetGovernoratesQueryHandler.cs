using ErrorOr;
using MediatR;
using PrimeFit.Application.Features.Branches.Shared.DTOS;
using PrimeFit.Domain.RepositoriesContracts;

namespace PrimeFit.Application.Features.Governorates.Queries.GetGovernorates
{
    public class GetGovernoratesQueryHandler : IRequestHandler<GetGovernoratesQuery, ErrorOr<List<GovernorateDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetGovernoratesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<List<GovernorateDto>>> Handle(GetGovernoratesQuery request, CancellationToken cancellationToken)
        {
            var spec = new GetGovernoratesSpec();
            var governorates = await _unitOfWork.Governorates.ListAsync<GovernorateDto>(spec, cancellationToken);

            return governorates;
        }
    }
}
