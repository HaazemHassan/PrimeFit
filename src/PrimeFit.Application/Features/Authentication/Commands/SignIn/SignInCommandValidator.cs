using FluentValidation;
using PrimeFit.Application.Common;
using PrimeFit.Application.Common.Options;
using PrimeFit.Application.Features.Users.Common;

namespace PrimeFit.Application.Features.Authentication.Commands.SignIn {
    public class SignInCommandValidator : AbstractValidator<SignInWithPasswordCommand> {
        public SignInCommandValidator(AppPasswordOptions passwordSettings) {
            ApplyValidationRules(passwordSettings);
        }

        private void ApplyValidationRules(AppPasswordOptions passwordSettings) {
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
