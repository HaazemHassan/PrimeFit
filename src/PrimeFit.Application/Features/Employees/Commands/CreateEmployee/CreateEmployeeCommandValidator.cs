using FluentValidation;
using Microsoft.Extensions.Options;
using PrimeFit.Application.Common;
using PrimeFit.Application.Common.Options;
using PrimeFit.Application.Features.Users.Common;
using PrimeFit.Application.ServicesContracts.Infrastructure;

namespace PrimeFit.Application.Features.Employees.Commands.CreateEmployee
{
    public class CreateEmployeeCommandValidator : AbstractValidator<CreateEmployeeCommand>
    {
        private readonly AppPasswordOptions _passwordOptions;

        public CreateEmployeeCommandValidator(IOptions<AppPasswordOptions> passwordOptions, IPhoneNumberService phoneNumberService)
        {
            _passwordOptions = passwordOptions.Value;

            RuleFor(x => x.BranchId).Required();
            RuleFor(x => x.EmployeeRoleId).Required();

            RuleFor(x => x.FirstName)
                .Required()
                .ApplyUserNameRules();

            RuleFor(x => x.LastName)
                .Required()
                .ApplyUserNameRules();

            RuleFor(x => x.Email)
                .Required()
                .ApplyEmailRules();

            RuleFor(x => x.PhoneNumber)
                .Required()
                .ApplyPhoneNumberRules(phoneNumberService);

            RuleFor(x => x.Password).Required()
                .ApplyPasswordRules(_passwordOptions);

            RuleFor(x => x.ConfirmPassword).ApplyConfirmPasswordRules(x => x.Password);

        }
    }
}

