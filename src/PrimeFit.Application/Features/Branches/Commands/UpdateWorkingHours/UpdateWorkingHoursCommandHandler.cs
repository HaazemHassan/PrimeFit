using AutoMapper;
using ErrorOr;
using MediatR;
using PrimeFit.Application.Contracts.Api;
using PrimeFit.Domain.Common.Constants;
using PrimeFit.Domain.Entities;
using PrimeFit.Domain.Repositories;
using PrimeFit.Domain.Specifications.Branches;

namespace PrimeFit.Application.Features.Branches.Commands.AddWorkingHours
{
    public class UpdateWorkingHoursCommandHandler : IRequestHandler<UpdateWorkingHoursCommand, ErrorOr<Success>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public UpdateWorkingHoursCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<ErrorOr<Success>> Handle(UpdateWorkingHoursCommand request, CancellationToken cancellationToken)
        {
            var getBranchWithWorkingHoursSpec = new GetBranchWithWorkingHoursSpec(request.BranchId);
            var branch = await _unitOfWork.Branches.FirstOrDefaultAsync(getBranchWithWorkingHoursSpec, cancellationToken);

            if (branch is null)
            {
                return Error.NotFound(ErrorCodes.Branch.BranchNotFound, "Branch not found");
            }

            if (!branch.IsOwner(_currentUserService.UserId!.Value))
            {
                return Error.Unauthorized();
            }



            var workingHours = new List<BranchWorkingHour>();

            foreach (var wh in request.WorkingHours)
            {
                var createResult = BranchWorkingHour.Create(wh.Day, wh.OpenTime, wh.CloseTime, wh.IsClosed, request.BranchId);

                if (createResult.IsError)
                {
                    return createResult.Errors;
                }
                workingHours.Add(createResult.Value);

            }

            branch.SetWorkingHours(workingHours);
            return Result.Success;
        }
    }
}
