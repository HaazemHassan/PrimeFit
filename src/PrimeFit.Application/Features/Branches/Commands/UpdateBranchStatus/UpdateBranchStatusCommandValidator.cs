using FluentValidation;
using PrimeFit.Application.Features.Branches.Commands.ToggleBranchStatus;

namespace PrimeFit.Application.Features.Branches.Commands.UpdateBranchStatus
{
    internal class UpdateBranchStatusCommandValidator : AbstractValidator<UpdateBranchStatusCommand>
    {
        public UpdateBranchStatusCommandValidator()
        {
            RuleFor(x => x.BranchId)
                .GreaterThan(0).WithMessage("BranchId must be greater than 0.");

            RuleFor(x => x.BranchStatus)
                .IsInEnum().WithMessage("Invalid branch status value.");

        }
    }
}
