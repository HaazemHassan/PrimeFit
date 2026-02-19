using FluentValidation;
using PrimeFit.Application.Common.Options;
using PrimeFit.Application.ValidationRules;
using PrimeFit.Application.ValidationRules.Common;

namespace PrimeFit.Application.Features.Authentication.Commands.SignIn {
    public class SignInCommandValidator : AbstractValidator<SignInCommand> {
        public SignInCommandValidator(PasswordSettings passwordSettings) {
            ApplyValidationRules(passwordSettings);
        }

        private void ApplyValidationRules(PasswordSettings passwordSettings) {
            RuleFor(x => x.Email).Required();
            RuleFor(x => x.Password).Required();




            When(x => !string.IsNullOrWhiteSpace(x.Email), () => {
                RuleFor(x => x.Email).ApplyEmailRules();
            });

            When(x => !string.IsNullOrWhiteSpace(x.Password), () => {
                RuleFor(x => x.Password).ApplyPasswordRules(passwordSettings);
            });
        }
    }
}
