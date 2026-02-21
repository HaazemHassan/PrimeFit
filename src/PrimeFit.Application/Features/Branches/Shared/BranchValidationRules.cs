using FluentValidation;

namespace PrimeFit.Application.Features.Users.Common
{
    public static class BranchValidationRules
    {
        public static IRuleBuilderOptions<T, string?> ApplyBranchNameRules<T>(
            this IRuleBuilder<T, string?> ruleBuilder,
            int minLength = 2,
            int maxLength = 25
        )
        {
            var rule = ruleBuilder;

            return rule
                .MinimumLength(minLength)
                    .WithMessage($"{{PropertyName}} must be at least {minLength} characters")
                .MaximumLength(maxLength)
                    .WithMessage($"{{PropertyName}} cannot exceed {maxLength} characters");

        }

    }
}

