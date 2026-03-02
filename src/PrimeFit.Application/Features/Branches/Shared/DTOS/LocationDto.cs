namespace PrimeFit.Application.Features.Branches.Shared.DTOS
{
    public class LocationDto
    {
        public string Address { get; set; } = null!;
        public CoordinatesDto Coordinates { get; set; } = null!;
        public GovernorateDto Governorate { get; set; } = null!;
    }
}

