using FluentValidation;
using PrimeFit.Application.Common;
using PrimeFit.Application.Features.Users.Common;
using PrimeFit.Application.ServicesContracts.Infrastructure;

namespace PrimeFit.Application.Features.Employees.Commands.UpdateEmployee
{
    public class UpdateEmployeeCommandValidator : AbstractValidator<UpdateEmployeeCommand>
    {

        public UpdateEmployeeCommandValidator(IPhoneNumberService phoneNumberService)
        {

            RuleFor(x => x.EmployeeId).Required();
            RuleFor(x => x.BranchId).Required();
            RuleFor(x => x.EmployeeRoleId)
                .GreaterThan(0)
                .When(x => x.EmployeeRoleId.HasValue);

            RuleFor(x => x.FirstName)
                .ApplyUserNameRules()
                .When(x => !string.IsNullOrWhiteSpace(x.FirstName));

            RuleFor(x => x.LastName)
                .ApplyUserNameRules()
                .When(x => !string.IsNullOrWhiteSpace(x.LastName));

            RuleFor(x => x.Email)
                .ApplyEmailRules()
                .When(x => !string.IsNullOrWhiteSpace(x.Email));

            RuleFor(x => x.PhoneNumber)
                .ApplyPhoneNumberRules(phoneNumberService)
                .When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber));


        }
    }
}
