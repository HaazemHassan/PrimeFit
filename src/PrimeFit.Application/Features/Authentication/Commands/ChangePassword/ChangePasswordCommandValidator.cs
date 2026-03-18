using FluentValidation;
using Microsoft.Extensions.Options;
using PrimeFit.Application.Common;
using PrimeFit.Application.Common.Options;
using PrimeFit.Application.Features.Users.Common;

namespace PrimeFit.Application.Features.Authentication.Commands.ChangePassword
{
    public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
    {
        private readonly AppPasswordOptions _passwordOptions;

        public ChangePasswordCommandValidator(IOptions<AppPasswordOptions> passwordOptions)
        {
            _passwordOptions = passwordOptions.Value;
            ApplyValidationRules();
        }

        private void ApplyValidationRules()
        {
            RuleFor(x => x.CurrentPassword).Required();
            RuleFor(x => x.NewPassword).Required();
            RuleFor(x => x.ConfirmNewPassword).Required();



            When(x => !string.IsNullOrWhiteSpace(x.CurrentPassword), () =>
            {
                RuleFor(x => x.CurrentPassword)
                    .ApplyPasswordRules(_passwordOptions);
            });

            When(x => !string.IsNullOrWhiteSpace(x.NewPassword), () =>
            {
                RuleFor(x => x.NewPassword)
                    .ApplyPasswordRules(_passwordOptions);
            });

            When(x => !string.IsNullOrWhiteSpace(x.NewPassword) && !string.IsNullOrWhiteSpace(x.ConfirmNewPassword), () =>
            {
                RuleFor(x => x.ConfirmNewPassword)
                    .ApplyConfirmPasswordRules(x => x.NewPassword);
            });
        }
    }
}
