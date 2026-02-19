namespace PrimeFit.Application.Security.Markers
{

    public interface IOwnedResourceRequest : IAuthorizedRequest
    {
        public int OwnerUserId { get; }
    }
}
