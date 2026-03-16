using PrimeFit.Application.Features.Branches.Shared.DTOS;
using PrimeFit.Domain.Common.Enums;

namespace PrimeFit.Application.Features.Branches.Queries.GetBranchSetupDetails
{
    public class GetBranchSetupDetailsQueryResponse
    {
        public BranchBussinessDetailsDto BussinessDetails { get; set; } = null!;
        public BranchLocationDetailsDto Location { get; set; } = null!;
        public List<WorkingHoursDTO> WorkingHours { get; set; }
        public List<ImageDto> Images { get; set; }
    }

    public class BranchLocationDetailsDto
    {
        public GovernorateDto? Governorate { get; set; }
        public string? Address { get; set; }
        public CoordinatesDto? Coordinates { get; set; }
    }

    public class BranchBussinessDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public BranchType BranchType { get; set; }

    }
}
