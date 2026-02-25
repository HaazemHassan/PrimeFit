using FluentValidation;
using PrimeFit.Application.Common;
using PrimeFit.Application.Features.Branches.Shared;

namespace PrimeFit.Application.Features.Branches.Commands.UpdatePackage
{
    public class UpdatePackageCommandValidator : AbstractValidator<UpdatePackageCommand>
    {
        public UpdatePackageCommandValidator()
        {
            ApplyValidationRules();
        }

        private void ApplyValidationRules()
        {
            RuleFor(x => x.BranchId).Required();
            RuleFor(x => x.PackageId).Required();
            RuleFor(x => x.Name).Required();

            When(x => !string.IsNullOrWhiteSpace(x.Name), () =>
            {
                RuleFor(x => x.Name).ApplyPackageNameRules();
            });

            RuleFor(x => x.Price).ApplyPriceRules();
            RuleFor(x => x.DurationInMonths).ApplyDurationInMonthsRules();
            RuleFor(x => x.NumberOfFreezes).ApplyNumberOfFreezesRules();
            RuleFor(x => x.FreezeDurationInDays).ApplyFreezeDurationInDaysRules();
        }
    }
}
