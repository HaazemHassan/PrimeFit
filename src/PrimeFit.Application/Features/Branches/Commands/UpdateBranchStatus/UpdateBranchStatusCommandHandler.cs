using ErrorOr;
using MediatR;
using PrimeFit.Application.Contracts.Api;
using PrimeFit.Application.Specifications.Branches;
using PrimeFit.Domain.Common.Constants;
using PrimeFit.Domain.Common.Enums;
using PrimeFit.Domain.Repositories;

namespace PrimeFit.Application.Features.Branches.Commands.ToggleBranchStatus
{
    public class UpdateBranchStatusCommandHandler : IRequestHandler<UpdateBranchStatusCommand, ErrorOr<Success>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public UpdateBranchStatusCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<ErrorOr<Success>> Handle(UpdateBranchStatusCommand request, CancellationToken cancellationToken)
        {
            var ownerId = _currentUserService.UserId!.Value;

            var spec = new BranchWithBasicDetailsSpec(request.BranchId);
            var branch = await _unitOfWork.Branches.FirstOrDefaultAsync(spec, cancellationToken);

            if (branch is null)
            {
                return Error.NotFound(
                    ErrorCodes.Branch.BranchNotFound,
                    "Branch not found.");
            }



            ErrorOr<Success> updateStatusResult;

            if (request.BranchStatus == BranchStatus.Active)
                updateStatusResult = branch.Activate();
            else
                updateStatusResult = branch.DeActivate();



            if (updateStatusResult.IsError)
            {
                return updateStatusResult.Errors;
            }


            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success;
        }
    }
}
