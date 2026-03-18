using FluentValidation;
using Microsoft.Extensions.Options;
using PrimeFit.Application.Common;
using PrimeFit.Application.Common.Options;
using PrimeFit.Application.Features.Users.Common;

namespace PrimeFit.Application.Features.Authentication.Commands.SignIn
{
    public class SignInCommandValidator : AbstractValidator<SignInWithPasswordCommand>
    {

        private readonly AppPasswordOptions _passwordOptions;
        public SignInCommandValidator(IOptions<AppPasswordOptions> passwordOptions)
        {
            _passwordOptions = passwordOptions.Value;
            ApplyValidationRules();
        }

        private void ApplyValidationRules()
        {
            RuleFor(x => x.Email).Required();
            RuleFor(x => x.Password).Required();




            When(x => !string.IsNullOrWhiteSpace(x.Email), () =>
            {
                RuleFor(x => x.Email).ApplyEmailRules();
            });

            When(x => !string.IsNullOrWhiteSpace(x.Password), () =>
            {
                RuleFor(x => x.Password).ApplyPasswordRules(_passwordOptions);
            });
        }
    }
}
