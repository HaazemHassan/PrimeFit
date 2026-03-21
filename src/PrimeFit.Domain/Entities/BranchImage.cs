using ErrorOr;
using PrimeFit.Domain.Common.Enums;
using PrimeFit.Domain.Entities.Base;

namespace PrimeFit.Domain.Entities
{
    public class BranchImage : AuditableEntity<int>
    {
        public BranchImage(string url, string publicId, BranchImageType type, int branchId, int displayOrder)
        {
            Status = BranchImageStatus.Pending;



            Url = url;
            PublicId = publicId;
            Type = type;
            BranchId = branchId;
            DisplayOrder = displayOrder;
        }


        public string Url { get; private set; } = string.Empty;
        public string PublicId { get; private set; } = string.Empty;
        public BranchImageType Type { get; private set; }
        public BranchImageStatus Status { get; private set; }
        public int DisplayOrder { get; private set; }


        public int BranchId { get; private set; }
        public Branch Branch { get; private set; } = null!;





        public ErrorOr<Success> UpdateImage(string newUrl, string newPublicId)
        {
            if (string.IsNullOrWhiteSpace(newUrl))
            {
                return Error.Validation(description: "Image URL cannot be empty.");
            }
            Url = newUrl;
            PublicId = newPublicId;
            return Result.Success;
        }



        public ErrorOr<Success> SetStatus(BranchImageStatus newStatus)
        {
            if (Status == BranchImageStatus.Replaced)
            {
                return Error.Validation(description: "Cannot change status of a replaced image.");
            }

            Status = newStatus;
            return Result.Success;
        }
    }
}
