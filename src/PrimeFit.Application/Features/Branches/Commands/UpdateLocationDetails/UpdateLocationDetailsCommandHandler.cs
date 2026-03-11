using ErrorOr;
using MediatR;
using PrimeFit.Application.Contracts.Api;
using PrimeFit.Application.Security.Contracts;
using PrimeFit.Domain.Common.Constants;
using PrimeFit.Domain.Common.Enums;
using PrimeFit.Domain.Repositories;
using PrimeFit.Domain.ValueObjects;

namespace PrimeFit.Application.Features.Branches.Commands.UpdateLocationDetails
{
    public class UpdateLocationDetailsCommandHandler : IRequestHandler<UpdateLocationDetailsCommand, ErrorOr<Success>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IBranchAuthorizationService _branchAuthorizationService;

        public UpdateLocationDetailsCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IBranchAuthorizationService branchAuthorizationService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _branchAuthorizationService = branchAuthorizationService;
        }

        public async Task<ErrorOr<Success>> Handle(UpdateLocationDetailsCommand request, CancellationToken cancellationToken)
        {
            var authResult = await _branchAuthorizationService.AuthorizeAsync(request.BranchId, Permission.BranchDetailsWrite, cancellationToken);
            if (authResult.IsError)
            {
                return authResult.Errors;
            }

            var geoLocatioResult = GeoLocation.Create(request.Latitude, request.Longitude);
            if (geoLocatioResult.IsError)
            {
                return geoLocatioResult.Errors;
            }
            var geoLocation = geoLocatioResult.Value;

            var branch = await _unitOfWork.Branches.GetByIdAsync(request.BranchId, cancellationToken);

            if (branch is null)
            {
                return Error.NotFound(ErrorCodes.Branch.BranchNotFound
                    , "Branch not found");
            }

            var governorate = await _unitOfWork.Governorates.GetByIdAsync(request.GovernorateId, cancellationToken);

            if (governorate is null)
            {
                return Error.NotFound(ErrorCodes.Branch.GovernorateNotFound
                    , "Governorate not found");
            }

            branch.UpdateLocationDetails(governorate, request.Address, geoLocation);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success;
        }
    }
}
