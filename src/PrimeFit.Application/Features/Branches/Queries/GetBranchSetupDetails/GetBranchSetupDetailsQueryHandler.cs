using ErrorOr;
using MediatR;
using PrimeFit.Application.Security.Contracts;
using PrimeFit.Domain.Common.Constants;
using PrimeFit.Domain.RepositoriesContracts;

namespace PrimeFit.Application.Features.Branches.Queries.GetBranchSetupDetails
{
    public class GetBranchSetupDetailsQueryHandler : IRequestHandler<GetBranchSetupDetailsQuery, ErrorOr<GetBranchSetupDetailsQueryResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetBranchSetupDetailsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<GetBranchSetupDetailsQueryResponse>> Handle(GetBranchSetupDetailsQuery request, CancellationToken cancellationToken)
        {

            var branch = await _unitOfWork.Branches.GetAsync<GetBranchSetupDetailsQueryResponse>(
                b => b.Id == request.BranchId,
                cancellationToken);

            if (branch is null)
            {
                return Error.NotFound(
                    ErrorCodes.Branch.BranchNotFound,
                    "Branch not found");
            }

            return branch;
        }
    }
}
