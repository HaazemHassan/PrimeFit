using FluentValidation;
using PrimeFit.Application.Common;

namespace PrimeFit.Application.Features.Branches.Commands.AddBranchImage
{
    public class AddBranchImageCommandValidator : AbstractValidator<AddBranchImageCommand>
    {
        public AddBranchImageCommandValidator()
        {
            ApplyValidationRules();
        }

        private void ApplyValidationRules()
        {
            RuleFor(x => x.BranchId).Required();
            RuleFor(x => x.ImageType).IsInEnum();
            RuleFor(x => x.ImageStream).NotEmpty();


        }

    }
}
