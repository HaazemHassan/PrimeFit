namespace PrimeFit.Api.Requests.BranchPackages.GetBranchPackagesForCustomers {
    public class GetBranchPackagesForCustomersRequest
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? Search { get; set; }
    }
}
