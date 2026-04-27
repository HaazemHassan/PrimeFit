using ErrorOr;
using MediatR;
using PrimeFit.Application.Security.Contracts;
using PrimeFit.Application.Specifications.BranchPackages;
using PrimeFit.Domain.Common.Constants;
using PrimeFit.Domain.Common.Enums;
using PrimeFit.Domain.RepositoriesContracts;

namespace PrimeFit.Application.Features.BranchPackages.Commands.UpdatePackageStatus
{
    public class UpdatePackageStatusCommandHandler : IRequestHandler<UpdatePackageStatusCommand, ErrorOr<Success>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdatePackageStatusCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<Success>> Handle(UpdatePackageStatusCommand request, CancellationToken cancellationToken)
        {

            var spec = new BranchPackageByIdSpec(request.PackageId, request.BranchId);

            var package = await _unitOfWork.Packages.FirstOrDefaultAsync(spec, cancellationToken);

            if (package is null)
            {
                return Error.NotFound(
                    ErrorCodes.Branch.PackageNotFound,
                    "Package not found");
            }

            var updateResult = package.UpdateStatus(request.IsActive);

            if (updateResult.IsError)
            {
                return updateResult.Errors;
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success;

        }
    }
}
