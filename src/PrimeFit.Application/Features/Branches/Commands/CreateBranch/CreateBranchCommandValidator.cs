using FluentValidation;
using PrimeFit.Application.Common;
using PrimeFit.Application.Common.Options;
using PrimeFit.Application.Features.Branches.Commands.CreateBranch;
using PrimeFit.Application.Features.Users.Common;
using PrimeFit.Application.ServicesContracts.Infrastructure;

namespace PrimeFit.Application.Features.Branches.Commands.CreateBranchBussinessDetails
{
    public class CreateBranchCommandValidator : AbstractValidator<CreateBranchCommand>
    {
        private readonly IPhoneNumberService _phoneNumberService;
        private readonly AppPasswordOptions _passwordSettings;
        public CreateBranchCommandValidator(AppPasswordOptions passwordSettings, IPhoneNumberService phoneNumberService)
        {
            _passwordSettings = passwordSettings;
            _phoneNumberService = phoneNumberService;

            ApplyValidationRules();
        }

        private void ApplyValidationRules()
        {
            RuleFor(x => x.Name).Required();
            RuleFor(x => x.Email).Required();
            RuleFor(x => x.PhoneNumber).Required();
            RuleFor(x => x.BranchType).IsInEnum();




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



        }

    }
}
