using FluentValidation;
using PrimeFit.Application.Common;
using PrimeFit.Application.Features.Users.Common;
using PrimeFit.Application.ServicesContracts.Infrastructure;

namespace PrimeFit.Application.Features.Branches.Commands.CreateMemberWithSubscription
{
    public class CreateMemberWithSubscriptionCommandValidator : AbstractValidator<CreateMemberWithSubscriptionCommand>
    {

        private readonly IPhoneNumberService _phoneNumberService;
        public CreateMemberWithSubscriptionCommandValidator(IPhoneNumberService phoneNumberService)
        {

            _phoneNumberService = phoneNumberService;


        }


        public void ApplyValidationRules()
        {
            RuleFor(x => x.FirstName).ApplyUserNameRules();
            RuleFor(x => x.LastName).ApplyUserNameRules();
            RuleFor(x => x.PhoneNumber).ApplyPhoneNumberRules(_phoneNumberService);
            RuleFor(x => x.Email).ApplyEmailRules();
        }
    }
}
