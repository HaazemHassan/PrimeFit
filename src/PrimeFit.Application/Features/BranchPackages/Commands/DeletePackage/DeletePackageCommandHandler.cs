using ErrorOr;
using MediatR;
using PrimeFit.Application.Features.BranchPackages.Commands.DeletePackage;
using PrimeFit.Application.Security.Contracts;
using PrimeFit.Application.Specifications.BranchPackages;
using PrimeFit.Domain.Common.Constants;
using PrimeFit.Domain.Common.Enums;
using PrimeFit.Domain.RepositoriesContracts;

namespace PrimeFit.Application.Features.Packages.Commands.DeletePackage
{
    public class DeletePackageCommandHandler : IRequestHandler<DeletePackageCommand, ErrorOr<Deleted>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBranchAuthorizationService _branchAuthorizationService;

        public DeletePackageCommandHandler(IUnitOfWork unitOfWork, IBranchAuthorizationService branchAuthorizationService)
        {
            _unitOfWork = unitOfWork;
            _branchAuthorizationService = branchAuthorizationService;
        }

        public async Task<ErrorOr<Deleted>> Handle(DeletePackageCommand request, CancellationToken cancellationToken)
        {
            var authResult = await _branchAuthorizationService.AuthorizeAsync(request.BranchId, Permission.PackagesDelete, cancellationToken);
            if (authResult.IsError)
            {
                return authResult.Errors;
            }

            var spec = new BranchPackageByIdSpec(request.PackageId, request.BranchId);
            var package = await _unitOfWork.Packages.FirstOrDefaultAsync(spec, cancellationToken);

            if (package is null)
            {
                return Error.NotFound(
                    ErrorCodes.Branch.PackageNotFound,
                    "Package not found");
            }

            await _unitOfWork.Packages.DeleteAsync(package, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Deleted;
        }
    }
}
