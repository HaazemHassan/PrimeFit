namespace PrimeFit.Application.Features.Branches.Shared.DTOS
{
    public class PackageDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public int DurationInMonths { get; set; }
        public int NumberOfFreezes { get; set; }
        public int FreezeDurationInDays { get; set; }
    }
}
