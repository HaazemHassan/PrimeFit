using AutoMapper;
using ErrorOr;
using MediatR;
using PrimeFit.Application.Features.Packages.Commands.UpdatePackage;
using PrimeFit.Application.Security.Contracts;
using PrimeFit.Application.Specifications.BranchPackages;
using PrimeFit.Domain.Common.Constants;
using PrimeFit.Domain.Common.Enums;
using PrimeFit.Domain.RepositoriesContracts;

namespace PrimeFit.Application.Features.BranchPackages.Commands.UpdatePackage
{
    public class UpdatePackageCommandHandler : IRequestHandler<UpdatePackageCommand, ErrorOr<UpdatePackageCommandResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IBranchAuthorizationService _branchAuthorizationService;

        public UpdatePackageCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IBranchAuthorizationService branchAuthorizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _branchAuthorizationService = branchAuthorizationService;
        }

        public async Task<ErrorOr<UpdatePackageCommandResponse>> Handle(UpdatePackageCommand request, CancellationToken cancellationToken)
        {
            var authResult = await _branchAuthorizationService.AuthorizeAsync(request.BranchId, Permission.PackagesWrite, cancellationToken);
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

            var updatePackageResult = package.Update(
                request.Name,
                request.Price,
                request.DurationInMonths,
                request.NumberOfFreezes,
                request.FreezeDurationInDays);

            if (updatePackageResult.IsError)
            {
                return updatePackageResult.Errors;
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var response = _mapper.Map<UpdatePackageCommandResponse>(updatePackageResult.Value);
            return response;
        }
    }
}
