using ErrorOr;
using PrimeFit.Domain.Common.Enums;

namespace PrimeFit.Domain.Entities
{
    public class BranchImage
    {
        public int Id { get; private set; }

        public string Url { get; private set; } = string.Empty;
        public string PublicId { get; private set; } = string.Empty;
        public BranchImageType Type { get; private set; }


        public int BranchId { get; private set; }
        public Branch Branch { get; private set; } = null!;


        private BranchImage() { }

        public static BranchImage Create(string url, string publicId, BranchImageType type, int branchId)
        {
            return new()
            {
                Url = url,
                PublicId = publicId,
                Type = type,
                BranchId = branchId
            };

        }


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
    }
}
