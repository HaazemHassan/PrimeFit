namespace PrimeFit.Application.ServicesContracts.Infrastructure.Cashing
{
    public interface ICacheService
    {
        Task<T> GetOrCreateAsync<T>(
            string key,
            Func<CancellationToken, Task<T>> factory,
            TimeSpan? expiration = null,
            CancellationToken ct = default);

        Task RemoveAsync(
            string key,
            CancellationToken ct = default);
    }
}
