namespace PrimeFit.API.Requests.Owner.Branches.UpdatePackage
{
    public class UpdatePackageRequest
    {
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public int DurationInMonths { get; set; }
        public bool IsActive { get; set; }
        public int NumberOfFreezes { get; set; }
        public int FreezeDurationInDays { get; set; }
    }
}
