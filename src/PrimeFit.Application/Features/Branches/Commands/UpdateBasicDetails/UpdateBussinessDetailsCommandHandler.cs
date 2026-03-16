using ErrorOr;
using MediatR;
using PrimeFit.Application.Contracts.Api;
using PrimeFit.Application.Security.Contracts;
using PrimeFit.Application.ServicesContracts.Infrastructure;
using PrimeFit.Domain.Common.Constants;
using PrimeFit.Domain.Common.Enums;
using PrimeFit.Domain.RepositoriesContracts;

namespace PrimeFit.Application.Features.Branches.Commands.UpdateBasicDetails
{
    public class UpdateBussinessDetailsCommandHandler : IRequestHandler<UpdateBussinessDetailsCommand, ErrorOr<Success>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IImageService _imageService;
        private readonly IBranchAuthorizationService _branchAuthorizationService;


        public UpdateBussinessDetailsCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            IImageService imageService,
            IImageBackgroundService imageBackgroundQueue,
            IBranchAuthorizationService branchAuthorizationService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _imageService = imageService;
            _branchAuthorizationService = branchAuthorizationService;
        }


        public async Task<ErrorOr<Success>> Handle(UpdateBussinessDetailsCommand request, CancellationToken cancellationToken)
        {
            var authResult = await _branchAuthorizationService.AuthorizeAsync(request.BranchId, Permission.BranchDetailsWrite, cancellationToken);
            if (authResult.IsError)
            {
                return authResult.Errors;
            }

            var branch = await _unitOfWork.Branches.GetByIdAsync(request.BranchId, cancellationToken);

            if (branch is null)
            {
                return Error.NotFound(
                    ErrorCodes.Branch.BranchNotFound,
                    "Branch not found");
            }

            branch.UpdateBasicDetails(request.Name, request.Email, request.PhoneNumber, request.BranchType);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success;


        }
    }
}
