using AutoMapper;
using ErrorOr;
using MediatR;
using PrimeFit.Application.Contracts.Api;
using PrimeFit.Application.Features.Branches.Commands.UpdateWorkingHours;
using PrimeFit.Application.Security.Contracts;
using PrimeFit.Domain.Common.Constants;
using PrimeFit.Domain.Common.Enums;
using PrimeFit.Domain.Entities;
using PrimeFit.Domain.RepositoriesContracts;
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

            branch.UpdateWorkingHours(workingHours);
            return Result.Success;
        }
    }
}
