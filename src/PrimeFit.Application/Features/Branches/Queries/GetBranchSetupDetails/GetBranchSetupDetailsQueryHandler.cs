using ErrorOr;
using MediatR;
using PrimeFit.Application.Security.Contracts;
using PrimeFit.Domain.Common.Constants;
using PrimeFit.Domain.Common.Enums;
using PrimeFit.Domain.RepositoriesContracts;

namespace PrimeFit.Application.Features.Branches.Queries.GetBranchSetupDetails
{
    public class GetBranchSetupDetailsQueryHandler : IRequestHandler<GetBranchSetupDetailsQuery, ErrorOr<GetBranchSetupDetailsQueryResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBranchAuthorizationService _branchAuthorizationService;

        public GetBranchSetupDetailsQueryHandler(IUnitOfWork unitOfWork, IBranchAuthorizationService branchAuthorizationService)
        {
            _unitOfWork = unitOfWork;
            _branchAuthorizationService = branchAuthorizationService;
        }

        public async Task<ErrorOr<GetBranchSetupDetailsQueryResponse>> Handle(GetBranchSetupDetailsQuery request, CancellationToken cancellationToken)
        {
            var authResult = await _branchAuthorizationService.AuthorizeAsync(
                request.BranchId,
                Permission.BranchDetailsRead,
                cancellationToken);

            if (authResult.IsError)
            {
                return authResult.Errors;
            }

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
