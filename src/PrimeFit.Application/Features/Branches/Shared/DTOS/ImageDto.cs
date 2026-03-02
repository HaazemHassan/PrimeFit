using PrimeFit.Domain.Common.Enums;

namespace PrimeFit.Application.Features.Branches.Shared.DTOS
{
    public class ImageDto
    {
        public int Id { get; set; }
        public BranchImageType Type { get; set; }
        public string Url { get; set; } = null!;
    }
}

