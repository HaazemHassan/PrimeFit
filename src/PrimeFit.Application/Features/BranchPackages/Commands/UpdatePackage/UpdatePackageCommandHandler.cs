using AutoMapper;
using ErrorOr;
using MediatR;
using PrimeFit.Application.Contracts.Api;
using PrimeFit.Application.Specifications.Packages;
using PrimeFit.Domain.Common.Constants;
using PrimeFit.Domain.Repositories;

namespace PrimeFit.Application.Features.Packages.Commands.UpdatePackage
{
    public class UpdatePackageCommandHandler : IRequestHandler<UpdatePackageCommand, ErrorOr<UpdatePackageCommandResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public UpdatePackageCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<ErrorOr<UpdatePackageCommandResponse>> Handle(UpdatePackageCommand request, CancellationToken cancellationToken)
        {

            var currentUserId = _currentUserService.UserId!.Value;

            var packageForOwnerSpec = new PackageByIdForOwnerSpec(request.PackageId, request.BranchId, currentUserId);

            var package = await _unitOfWork.Packages.FirstOrDefaultAsync(packageForOwnerSpec, cancellationToken);

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
