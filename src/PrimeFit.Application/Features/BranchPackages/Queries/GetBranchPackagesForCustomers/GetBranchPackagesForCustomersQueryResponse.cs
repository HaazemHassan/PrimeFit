namespace PrimeFit.Application.Features.Packages.Queries.GetBranchPackagesForCustomers
{
    public class GetBranchPackagesForCustomersQueryResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public int DurationInMonths { get; set; }
        public int NumberOfFreezes { get; set; }
        public int FreezeDurationInDays { get; set; }
    }
}
