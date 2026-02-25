using ErrorOr;
using MediatR;
using PrimeFit.Application.Contracts.Api;
using PrimeFit.Domain.Common.Constants;
using PrimeFit.Domain.Repositories;
using PrimeFit.Domain.ValueObjects;

namespace PrimeFit.Application.Features.Branches.Commands.UpdateLocationDetails
{
    public class UpdateLocationDetailsCommandHandler : IRequestHandler<UpdateLocationDetailsCommand, ErrorOr<Success>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public UpdateLocationDetailsCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<ErrorOr<Success>> Handle(UpdateLocationDetailsCommand request, CancellationToken cancellationToken)
        {

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

            if (!branch.IsOwner(_currentUserService.UserId!.Value))
            {
                return Error.Unauthorized();
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
