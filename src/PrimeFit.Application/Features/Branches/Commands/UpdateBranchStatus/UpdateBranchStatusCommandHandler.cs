using ErrorOr;
using MediatR;
using PrimeFit.Application.Contracts.Api;
using PrimeFit.Application.Features.Branches.Commands.UpdateBranchStatus;
using PrimeFit.Application.Security.Contracts;
using PrimeFit.Application.Specifications.Branches;
using PrimeFit.Domain.Common.Constants;
using PrimeFit.Domain.Common.Enums;
using PrimeFit.Domain.RepositoriesContracts;

namespace PrimeFit.Application.Features.Branches.Commands.ToggleBranchStatus
{
    public class UpdateBranchStatusCommandHandler : IRequestHandler<UpdateBranchStatusCommand, ErrorOr<Success>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IBranchAuthorizationService _branchAuthorizationService;

        public UpdateBranchStatusCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IBranchAuthorizationService branchAuthorizationService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _branchAuthorizationService = branchAuthorizationService;
        }

        public async Task<ErrorOr<Success>> Handle(UpdateBranchStatusCommand request, CancellationToken cancellationToken)
        {
            var authResult = await _branchAuthorizationService.AuthorizeAsync(request.BranchId, Permission.BranchDetailsWrite, cancellationToken);
            if (authResult.IsError)
            {
                return authResult.Errors;
            }

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
