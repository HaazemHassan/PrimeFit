using FluentValidation;
using PrimeFit.Application.Common;

namespace PrimeFit.Application.Features.Authentication.Queries.ValidateResetPasswordCode
{
    public class ValidateResetPasswordCodeQueryValidator : AbstractValidator<ValidateResetPasswordCodeQuery>
    {
        public ValidateResetPasswordCodeQueryValidator()
        {
            RuleFor(x => x.Email).Required();
            RuleFor(x => x.Code).Required();
        }
    }
}
