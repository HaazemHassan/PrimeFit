using ErrorOr;
using MediatR;
using PrimeFit.Application.Contracts.Api;
using PrimeFit.Application.Security.Contracts;
using PrimeFit.Application.ServicesContracts.Infrastructure;
using PrimeFit.Application.Specifications.Branches;
using PrimeFit.Domain.Common.Constants;
using PrimeFit.Domain.RepositoriesContracts;

namespace PrimeFit.Application.Features.Branches.Queries.GetBranchById
{
    public class GetBranchByIdQueryHandler : IRequestHandler<GetBranchByIdQuery, ErrorOr<GetBranchByIdQueryResponse>>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTimeProvider _dateTimeProvider;
        public GetBranchByIdQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IDateTimeProvider dateTimeProvider)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _dateTimeProvider = dateTimeProvider;
        }



        public async Task<ErrorOr<GetBranchByIdQueryResponse>> Handle(GetBranchByIdQuery request, CancellationToken cancellationToken)
        {

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

            branchResponse.IsOpenNow = branch.IsOpenNow(_dateTimeProvider.GetTimeZoneNow("Africa/Cairo"));

            return branchResponse;

        }
    }
}
