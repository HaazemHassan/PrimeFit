namespace PrimeFit.Application.Common.Caching
{
    public interface ICacheInvalidationPolicy<in TRequest>
    {
        IEnumerable<string> GetTags(TRequest request);
    }
}
