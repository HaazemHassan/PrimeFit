using FluentValidation;

namespace PrimeFit.Application.Features.Branches.Commands.UpdateBranchStatus
{
    public class UpdateBranchStatusValidator : AbstractValidator<UpdateBranchStatusCommand>
    {

        public UpdateBranchStatusValidator()
        {
            RuleFor(b => b.BranchStatus).IsInEnum()
                .WithMessage("Invalid branch status value.");
        }
    }
}
