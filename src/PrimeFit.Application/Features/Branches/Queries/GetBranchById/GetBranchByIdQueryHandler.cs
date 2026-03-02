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
            var branchForOwnerSpec = new BranchForOwnerSpec(request.BranchId, userId);

            //_unitOfWork.Branches.AsQueryable().pro

            var branchExists = await _unitOfWork.Branches.AnyAsync(branchForOwnerSpec, cancellationToken);
            if (!branchExists)
            {

                return Error.NotFound(
                    ErrorCodes.Branch.BranchNotFound, "Branch not found");

            }

            var branchSpec = new BranchDetailsSpec(request.BranchId, _timeProvider.GetUtcNow());

            var branchRespose = await _unitOfWork.Branches.GetAsync<GetBranchByIdQueryResponse>(branchSpec, cancellationToken);
            if (branchRespose is null)
            {
                return Error.NotFound(
                    ErrorCodes.Branch.BranchNotFound, "Branch not found");
            }


            return branchRespose;

        }
    }
}
