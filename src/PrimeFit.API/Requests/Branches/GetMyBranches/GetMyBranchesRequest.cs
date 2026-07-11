using ErrorOr;

namespace PrimeFit.Api.Requests.Branches.GetMyBranches;

public class GetMyBranchesRequest
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? Search { get; set; }
}
