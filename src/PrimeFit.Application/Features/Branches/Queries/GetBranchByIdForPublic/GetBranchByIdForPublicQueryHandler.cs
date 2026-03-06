using AutoMapper;
using ErrorOr;
using MediatR;
using PrimeFit.Application.Contracts.Api;
using PrimeFit.Application.ServicesContracts.Infrastructure;
using PrimeFit.Application.Specifications.Branches;
using PrimeFit.Domain.Common.Constants;
using PrimeFit.Domain.Common.Enums;
using PrimeFit.Domain.Repositories;

namespace PrimeFit.Application.Features.Branches.Queries.GetBranchByIdForPublic
{
    public class GetBranchByIdForPublicQueryHandler : IRequestHandler<GetBranchByIdForPublicQuery, ErrorOr<GetBranchByIdForPublicQueryResponse>>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IMapper _mapper;
        public GetBranchByIdForPublicQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IDateTimeProvider dateTimeProvider, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _dateTimeProvider = dateTimeProvider;
            _mapper = mapper;
        }



        public async Task<ErrorOr<GetBranchByIdForPublicQueryResponse>> Handle(GetBranchByIdForPublicQuery request, CancellationToken cancellationToken)
        {
            var branchWithWorkingHoursSpec = new BranchWithWorkingHoursSpec(request.BranchId, BranchStatus.Active);

            var branch = await _unitOfWork.Branches.FirstOrDefaultAsync(branchWithWorkingHoursSpec, cancellationToken);
            if (branch is null || branch.BranchStatus != BranchStatus.Active)
            {

                return Error.NotFound(
                    ErrorCodes.Branch.BranchNotFound, "Branch not found");

            }

            var branchResponseSpec = new GetBranchByIdForPublicQuerySpec(request.BranchId, request.Latitude, request.Longitude);
            var branchResponse = await _unitOfWork.Branches.FirstOrDefaultAsync(branchResponseSpec, cancellationToken);
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
