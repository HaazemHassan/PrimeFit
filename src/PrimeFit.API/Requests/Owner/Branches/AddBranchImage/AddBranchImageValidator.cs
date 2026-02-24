using FluentValidation;
using PrimeFit.API.Extensions;

namespace PrimeFit.API.Requests.Owner.Branches.AddBranchImage
{
    public class UploadBranchImageValidator : AbstractValidator<AddBranchImageRequest>
    {
        public UploadBranchImageValidator()
        {
            RuleFor(x => x.File).ApplyImageRules();
        }
    }
}
