using FluentValidation;
using PrimeFit.Application.Common;

namespace PrimeFit.Application.Features.Authentication.Commands.ConfirmEmail
{
    public class ConfirmEmailCommandValidator : AbstractValidator<ConfirmEmailCommand>
    {
        public ConfirmEmailCommandValidator()
        {
            RuleFor(x => x.code).Required();
        }
    }
}
