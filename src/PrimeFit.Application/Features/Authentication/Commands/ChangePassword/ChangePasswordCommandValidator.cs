using FluentValidation;
using PrimeFit.Application.Common;
using PrimeFit.Application.Common.Options;
using PrimeFit.Application.Features.Users.Common;

namespace PrimeFit.Application.Features.Authentication.Commands.ChangePassword {
    public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand> {
        public ChangePasswordCommandValidator(AppPasswordOptions passwordSettings) {
            ApplyValidationRules(passwordSettings);
        }

        private void ApplyValidationRules(AppPasswordOptions passwordSettings) {
            RuleFor(x => x.CurrentPassword).Required();
            RuleFor(x => x.NewPassword).Required();
            RuleFor(x => x.ConfirmNewPassword).Required();



            When(x => !string.IsNullOrWhiteSpace(x.CurrentPassword), () => {
                RuleFor(x => x.CurrentPassword)
                    .ApplyPasswordRules(passwordSettings);
            });

            When(x => !string.IsNullOrWhiteSpace(x.NewPassword), () => {
                RuleFor(x => x.NewPassword)
                    .ApplyPasswordRules(passwordSettings);
            });

            When(x => !string.IsNullOrWhiteSpace(x.NewPassword) && !string.IsNullOrWhiteSpace(x.ConfirmNewPassword), () => {
                RuleFor(x => x.ConfirmNewPassword)
                    .ApplyConfirmPasswordRules(x => x.NewPassword);
            });
        }
    }
}
