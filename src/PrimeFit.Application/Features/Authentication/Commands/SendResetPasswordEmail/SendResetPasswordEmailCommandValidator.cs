using FluentValidation;
using PrimeFit.Application.Common;

namespace PrimeFit.Application.Features.Authentication.Commands.SendResetPasswordEmail
{
    public class SendResetPasswordEmailCommandValidator : AbstractValidator<SendResetPasswordEmailCommand>
    {
        public SendResetPasswordEmailCommandValidator()
        {
            RuleFor(x => x.Email).Required();

            When(x => !string.IsNullOrEmpty(x.Email), () =>
            {
                RuleFor(x => x.Email).ApplyEmailRules();
            });
        }
    }
}
