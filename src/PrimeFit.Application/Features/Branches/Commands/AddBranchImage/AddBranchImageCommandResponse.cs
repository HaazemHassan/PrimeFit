using PrimeFit.Domain.Common.Enums;

namespace PrimeFit.Application.Features.Branches.Commands.CreateBranchImage
{
    public class AddBranchImageCommandResponse
    {


        public int BranchId { get; set; }
        public int ImageId { get; set; }
        public string ImageUrl { get; set; }
        public BranchImageStatus Status { get; set; }
        public int DisplayOrder { get; set; }

    }
}
