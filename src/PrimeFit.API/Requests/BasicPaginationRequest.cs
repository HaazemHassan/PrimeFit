namespace PrimeFit.API.Requests
{
    public class BasicPaginationRequest
    {
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
        public string? Search { get; set; }
        public string? SortBy { get; set; }
    }
}
