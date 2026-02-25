using FluentValidation;

namespace PrimeFit.Application.Features.Branches.Shared
{
    public static class PackageValidationRules
    {
        public static IRuleBuilderOptions<T, string?> ApplyPackageNameRules<T>(
            this IRuleBuilder<T, string?> ruleBuilder)
        {
            return ruleBuilder
                .MaximumLength(100)
                    .WithMessage("{PropertyName} cannot exceed 100 characters");
        }

        public static IRuleBuilderOptions<T, decimal> ApplyPriceRules<T>(
            this IRuleBuilder<T, decimal> ruleBuilder)
        {
            return ruleBuilder
                .GreaterThan(0)
                    .WithMessage("Price must be greater than 0");
        }

        public static IRuleBuilderOptions<T, int> ApplyDurationInMonthsRules<T>(
            this IRuleBuilder<T, int> ruleBuilder)
        {
            return ruleBuilder
                .GreaterThan(0)
                    .WithMessage("Duration must be greater than 0");
        }

        public static IRuleBuilderOptions<T, int> ApplyNumberOfFreezesRules<T>(
            this IRuleBuilder<T, int> ruleBuilder)
        {
            return ruleBuilder
                .GreaterThanOrEqualTo(0)
                    .WithMessage("Number of freezes cannot be negative");
        }

        public static IRuleBuilderOptions<T, int> ApplyFreezeDurationInDaysRules<T>(
            this IRuleBuilder<T, int> ruleBuilder)
        {
            return ruleBuilder
                .GreaterThanOrEqualTo(0)
                    .WithMessage("Freeze duration cannot be negative");
        }
    }
}
