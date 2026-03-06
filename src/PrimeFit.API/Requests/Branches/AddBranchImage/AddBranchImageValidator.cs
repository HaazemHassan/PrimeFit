using FluentValidation;
using PrimeFit.API.Extensions;

namespace PrimeFit.API.Requests.Branches.AddBranchImage
{
    public class AddBranchImageValidator : AbstractValidator<AddBranchImageRequest>
    {
        public AddBranchImageValidator()
        {
            RuleFor(x => x.ImageFile).ApplyImageRules();
        }
    }
}
