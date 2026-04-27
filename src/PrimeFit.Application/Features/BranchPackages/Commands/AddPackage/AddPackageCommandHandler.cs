using AutoMapper;
using ErrorOr;
using MediatR;
using PrimeFit.Application.Features.Packages.Commands.AddPackage;
using PrimeFit.Application.Security.Contracts;
using PrimeFit.Domain.Common.Constants;
using PrimeFit.Domain.Common.Enums;
using PrimeFit.Domain.RepositoriesContracts;

namespace PrimeFit.Application.Features.BranchPackages.Commands.AddPackage
{
    public class AddPackageCommandHandler : IRequestHandler<AddPackageCommand, ErrorOr<AddPackageCommandResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AddPackageCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ErrorOr<AddPackageCommandResponse>> Handle(AddPackageCommand request, CancellationToken cancellationToken)
        {

            var branch = await _unitOfWork.Branches.GetByIdAsync(request.BranchId, cancellationToken);

            if (branch is null)
            {
                return Error.NotFound(
                    ErrorCodes.Branch.BranchNotFound,
                    "Branch not found");
            }

            var addPackageResult = branch.AddPackage(
                request.Name,
                request.Price,
                request.DurationInMonths,
                request.IsActive,
                request.NumberOfFreezes,
                request.FreezeDurationInDays);

            if (addPackageResult.IsError)
            {
                return addPackageResult.Errors;
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var package = addPackageResult.Value;

            var response = _mapper.Map<AddPackageCommandResponse>(package);
            return response;
        }
    }
}
