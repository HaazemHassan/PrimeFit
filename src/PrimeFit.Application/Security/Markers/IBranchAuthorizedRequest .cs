namespace PrimeFit.Application.Security.Markers
{
    public interface IBranchAuthorizedRequest : IAuthorizedRequest
    {
        public int BranchId { get; }

    }
}
