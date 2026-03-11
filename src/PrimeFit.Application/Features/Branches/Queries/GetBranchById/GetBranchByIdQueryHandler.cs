using ErrorOr;
using MediatR;
using PrimeFit.Application.Contracts.Api;
using PrimeFit.Application.Security.Contracts;
using PrimeFit.Application.ServicesContracts.Infrastructure;
using PrimeFit.Application.Specifications.Branches;
using PrimeFit.Domain.Common.Constants;
using PrimeFit.Domain.Common.Enums;
using PrimeFit.Domain.Repositories;

namespace PrimeFit.Application.Features.Branches.Queries.GetBranchById
{
    public class GetBranchByIdQueryHandler : IRequestHandler<GetBranchByIdQuery, ErrorOr<GetBranchByIdQueryResponse>>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IBranchAuthorizationService _branchAuthorizationService;
        public GetBranchByIdQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IDateTimeProvider dateTimeProvider, IBranchAuthorizationService branchAuthorizationService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _dateTimeProvider = dateTimeProvider;
            _branchAuthorizationService = branchAuthorizationService;
        }



        public async Task<ErrorOr<GetBranchByIdQueryResponse>> Handle(GetBranchByIdQuery request, CancellationToken cancellationToken)
        {
            var authResult = await _branchAuthorizationService.AuthorizeAsync(request.BranchId, Permission.BranchDetailsRead, cancellationToken);
            if (authResult.IsError)
            {
                return authResult.Errors;
            }

            var branchWithWorkingHoursSpec = new BranchWithWorkingHoursSpec(request.BranchId);


            var branch = await _unitOfWork.Branches.FirstOrDefaultAsync(branchWithWorkingHoursSpec, cancellationToken);
            if (branch is null)
            {

                return Error.NotFound(
                    ErrorCodes.Branch.BranchNotFound, "Branch not found");

            }

            var branchResponse = await _unitOfWork.Branches.GetByIdAsync<GetBranchByIdQueryResponse>(request.BranchId, cancellationToken);
            if (branchResponse is null)
            {
                return Error.NotFound(
                    ErrorCodes.Branch.BranchNotFound, "Branch not found");
            }

            branchResponse.IsOpenNow = branch.IsOpenNow(_dateTimeProvider.GetNow("Africa/Cairo"));

            return branchResponse;

        }
    }
}
