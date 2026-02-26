namespace PrimeFit.Application.Features.BranchPackages.Queries.GetBranchPackagesForOwner
{
    public class GetBranchPackagesForOwnerQueryResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public int DurationInMonths { get; set; }
        public bool IsActive { get; set; }
        public int NumberOfFreezes { get; set; }
        public int FreezeDurationInDays { get; set; }
    }

    public record PackagesStatsMeta(int TotalPackageCount, int ActivePackagesCount, decimal AveragePrice);
}
