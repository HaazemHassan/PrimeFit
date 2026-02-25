namespace PrimeFit.Application.Features.Packages.Commands.AddPackage
{
    public class AddPackageCommandResponse
    {
        public int PackageId { get; set; }
        public string Name { get; set; } = default!;
        public decimal Price { get; set; }
        public int DurationInMonths { get; set; }
        public bool IsActive { get; set; }
        public int NumberOfFreezes { get; set; }
        public int FreezeDurationInDays { get; set; }


    }
}
