namespace PrimeFit.API.Requests.Owner.Branches.AddPackage
{
    public class AddPackageRequest
    {
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public int DurationInMonths { get; set; }
        public bool IsActive { get; set; }
        public int NumberOfFreezes { get; set; }
        public int FreezeDurationInDays { get; set; }
    }
}
