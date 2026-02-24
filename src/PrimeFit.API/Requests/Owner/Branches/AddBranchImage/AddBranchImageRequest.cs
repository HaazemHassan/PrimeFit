using PrimeFit.Domain.Common.Enums;

namespace PrimeFit.API.Requests.Owner.Branches.AddBranchImage
{
    public class AddBranchImageRequest
    {
        public IFormFile ImageFile { get; set; } = null!;
        public BranchImageType ImageType { get; set; }
    }
}
