namespace PrimeFit.Application.Common.Cashing
{
    public interface ICachePolicy<in TRequest>
    {
        string GetCacheKey(TRequest request);
        string[] GetCacheTags(TRequest request) => [];

        bool ShouldSkipCache(TRequest request)
        {
            return false;
        }

        TimeSpan? Expiration { get; }
    }
}
