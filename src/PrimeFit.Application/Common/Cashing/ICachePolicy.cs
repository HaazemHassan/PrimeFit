namespace PrimeFit.Application.Common.Cashing
{
    public interface ICachePolicy<in TRequest>
    {
        string GetCacheKey(TRequest request);

        TimeSpan? Expiration { get; }
    }
}
