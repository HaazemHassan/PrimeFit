using FluentValidation;
using PrimeFit.API.Extensions;

namespace PrimeFit.API.Requests.Owner.Branches.UpdateBranchImage
{
    public class UploadBranchImageValidator : AbstractValidator<UpdateBranchImageRequest>
    {
        public UploadBranchImageValidator()
        {
            RuleFor(x => x.ImageFile).ApplyImageRules();
        }
    }
}
