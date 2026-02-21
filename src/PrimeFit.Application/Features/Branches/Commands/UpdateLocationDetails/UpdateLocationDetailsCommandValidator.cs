using FluentValidation;
using PrimeFit.Application.Common;

namespace PrimeFit.Application.Features.Branches.Commands.UpdateLocationDetails
{
    public class UpdateLocationDetailsCommandValidator : AbstractValidator<UpdateLocationDetailsCommand>
    {
        public UpdateLocationDetailsCommandValidator()
        {
            ApplyValidationRules();
        }

        private void ApplyValidationRules()
        {
            RuleFor(x => x.Address).Required();
            RuleFor(x => x.BranchId).Required();
            RuleFor(x => x.GovernorateId).Required();


            When(x => !string.IsNullOrWhiteSpace(x.Address), () =>
            {
                RuleFor(x => x.Address).ApplyAddressRules();
            });
        }
    }
}
