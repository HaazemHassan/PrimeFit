using ErrorOr;
using MediatR;
using PrimeFit.Application.Contracts.Api;
using PrimeFit.Application.Specifications.BranchPackages;
using PrimeFit.Domain.Common.Constants;
using PrimeFit.Domain.Repositories;

namespace PrimeFit.Application.Features.Packages.Commands.DeletePackage
{
    public class DeletePackageCommandHandler : IRequestHandler<DeletePackageCommand, ErrorOr<Deleted>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public DeletePackageCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<ErrorOr<Deleted>> Handle(DeletePackageCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.UserId!.Value;

            var spec = new OwnerBranchPackageByIdSpec(request.PackageId, request.BranchId, currentUserId);
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
