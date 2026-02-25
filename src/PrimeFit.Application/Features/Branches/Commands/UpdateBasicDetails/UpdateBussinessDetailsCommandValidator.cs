using FluentValidation;
using PrimeFit.Application.Common;
using PrimeFit.Application.Features.Users.Common;
using PrimeFit.Application.ServicesContracts.Infrastructure;

namespace PrimeFit.Application.Features.Branches.Commands.UpdateBasicDetails
{
    public class UpdateBussinessDetailsCommandValidator : AbstractValidator<UpdateBussinessDetailsCommand>
    {
        private readonly IPhoneNumberService _phoneNumberService;

        public UpdateBussinessDetailsCommandValidator(IPhoneNumberService phoneNumberService)
        {
            _phoneNumberService = phoneNumberService;


            ApplyValidationRules();
            ApplyCustomValidations();

        }


        private void ApplyValidationRules()
        {
            RuleFor(x => x.BranchId)
                 .GreaterThan(0).WithMessage("Branch id is not valid");

            When(x => !string.IsNullOrWhiteSpace(x.Name), () =>
            {
                RuleFor(x => x.Name).ApplyBranchNameRules();
            });

            When(x => !string.IsNullOrWhiteSpace(x.Email), () =>
            {
                RuleFor(x => x.Email).ApplyEmailRules();
            });

            When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber), () =>
            {
                RuleFor(x => x.PhoneNumber).ApplyPhoneNumberRules(_phoneNumberService);
            });


            When(x => x.BranchType is not null, () =>
            {
                RuleFor(x => x.BranchType).IsInEnum();
            });

        }

        private void ApplyCustomValidations()
        {
            RuleFor(x => x)
               .Must(HaveAtLeastOneNonNullProperty)
               .WithMessage("Nothing to change.");
        }

        private bool HaveAtLeastOneNonNullProperty(UpdateBussinessDetailsCommand command)
        {
            return command.Name != null ||
                   command.Email != null ||
                   command.PhoneNumber != null ||
                   command.BranchType != null;
        }
    }



}
