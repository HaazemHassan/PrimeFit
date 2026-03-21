using FluentValidation;
using PrimeFit.Application.Common;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Features.Branches.Commands.ActivateBranchImages
{
    public class ActivateBranchImagesCommandValidator : AbstractValidator<ActivateBranchImagesCommand>
    {
        public ActivateBranchImagesCommandValidator()
        {
            ApplyValidationRules();
        }

        private void ApplyValidationRules()
        {
            RuleFor(x => x.BranchId).Required();
            RuleFor(x => x.Images).NotEmpty().
                Must(images => images.Count <= Branch.MaxImageCount)
                 .WithMessage($"Maximum allowed images is {Branch.MaxImageCount}."); ;
        }

    }
}
