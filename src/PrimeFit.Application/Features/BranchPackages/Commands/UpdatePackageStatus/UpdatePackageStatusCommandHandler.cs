using AutoMapper;
using ErrorOr;
using MediatR;
using PrimeFit.Application.Contracts.Api;
using PrimeFit.Application.Specifications.BranchPackages;
using PrimeFit.Domain.Common.Constants;
using PrimeFit.Domain.Repositories;

namespace PrimeFit.Application.Features.BranchPackages.Commands.UpdatePackageStatus
{
    public class UpdatePackageStatusCommandHandler : IRequestHandler<UpdatePackageStatusCommand, ErrorOr<Success>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public UpdatePackageStatusCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<ErrorOr<Success>> Handle(UpdatePackageStatusCommand request, CancellationToken cancellationToken)
        {

            var currentUserId = _currentUserService.UserId!.Value;

            var packageForOwnerSpec = new OwnerBranchPackageByIdSpec(request.PackageId, request.BranchId, currentUserId);

            var package = await _unitOfWork.Packages.FirstOrDefaultAsync(packageForOwnerSpec, cancellationToken);

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
