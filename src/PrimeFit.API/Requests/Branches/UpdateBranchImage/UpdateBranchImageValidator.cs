using FluentValidation;
using PrimeFit.API.Extensions;

namespace PrimeFit.Api.Requests.Branches.UpdateBranchImage {
    public class UploadBranchImageValidator : AbstractValidator<UpdateBranchImageRequest>
    {
        public UploadBranchImageValidator()
        {
            RuleFor(x => x.ImageFile).ApplyImageRules();
        }
    }
}
