using PrimeFit.Domain.Common.Enums;

namespace PrimeFit.API.Requests.Owner.Branches.AddBranchImage
{
    public class AddBranchImageRequest
    {
        public IFormFile File { get; set; } = null!;
        public BranchImageType ImageType { get; set; }
    }
}
