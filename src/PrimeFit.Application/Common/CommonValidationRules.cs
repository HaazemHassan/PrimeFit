using FluentValidation;
using PrimeFit.Application.ServicesContracts.Infrastructure;

namespace PrimeFit.Application.Common
{
    public static class CommonValidationRules
    {

        public static IRuleBuilderOptions<T, string?> Required<T>(
           this IRuleBuilder<T, string?> ruleBuilder
        )
        {
            var rule = ruleBuilder;

            return rule.NotEmpty()
                    .WithMessage("{PropertyName} can't be empty");

        }


        public static IRuleBuilderOptions<T, int> Required<T>(
           this IRuleBuilder<T, int> ruleBuilder
        )
        {
            var rule = ruleBuilder;
            return rule.GreaterThan(0).WithMessage("{PropertyName} can't be empty");
        }

        public static IRuleBuilderOptions<T, string?> ApplyEmailRules<T>(
            this IRuleBuilder<T, string> ruleBuilder
        )
        {
            var rule = ruleBuilder;
            return rule
                .MaximumLength(100)
                    .WithMessage("{PropertyName} cannot exceed 100 characters")
                .EmailAddress()
                    .WithMessage("Invalid email address format");
        }


        public static IRuleBuilderOptions<T, string?> ApplyPhoneNumberRules<T>(
            this IRuleBuilder<T, string?> ruleBuilder,
            IPhoneNumberService phoneNumberService
        )
        {
            var rule = ruleBuilder;

            return rule.Must(phone => string.IsNullOrWhiteSpace(phone) || phoneNumberService.IsValid(phone))
                    .WithMessage("Phone number is not valid");
        }


        public static IRuleBuilderOptions<T, string?> ApplyAddressRules<T>(
           this IRuleBuilder<T, string?> ruleBuilder,
           int minLength = 20,
           int maxLength = 200
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

