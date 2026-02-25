using AutoMapper;
using ErrorOr;
using MediatR;
using PrimeFit.Application.Contracts.Api;
using PrimeFit.Application.Specifications.Branches;
using PrimeFit.Domain.Common.Constants;
using PrimeFit.Domain.Repositories;

namespace PrimeFit.Application.Features.Branches.Commands.AddPackage
{
    public class AddPackageCommandHandler : IRequestHandler<AddPackageCommand, ErrorOr<AddPackageCommandResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public AddPackageCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<ErrorOr<AddPackageCommandResponse>> Handle(AddPackageCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.UserId!.Value;

            var branchForOwnerSpec = new BranchForOwnerSpec(request.BranchId, currentUserId);
            var branch = await _unitOfWork.Branches.FirstOrDefaultAsync(branchForOwnerSpec, cancellationToken);

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
