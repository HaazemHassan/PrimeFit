using ErrorOr;
using MediatR;
using PrimeFit.Application.Contracts.Api;
using PrimeFit.Application.Specifications.Branches;
using PrimeFit.Domain.Common.Constants;
using PrimeFit.Domain.Repositories;

namespace PrimeFit.Application.Features.Branches.Queries.GetBranchById
{
    public class GetBranchByIdQueryHandler : IRequestHandler<GetBranchByIdQuery, ErrorOr<GetBranchByIdQueryResponse>>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly TimeProvider _timeProvider;
        public GetBranchByIdQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, TimeProvider timeProvider)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _timeProvider = timeProvider;
        }



        public async Task<ErrorOr<GetBranchByIdQueryResponse>> Handle(GetBranchByIdQuery request, CancellationToken cancellationToken)
        {
            int userId = _currentUserService.UserId!.Value;
            var branchWithWorkingHoursSpec = new BranchWithWorkingHoursSpec(request.BranchId);

            //_unitOfWork.Branches.AsQueryable().pro

            var branch = await _unitOfWork.Branches.FirstOrDefaultAsync(branchWithWorkingHoursSpec, cancellationToken);
            if (branch is null || branch.OwnerId != userId)
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

            branchResponse.IsOpenNow = branch.IsOpenNow(_timeProvider.GetUtcNow());

            return branchResponse;

        }
    }
}
