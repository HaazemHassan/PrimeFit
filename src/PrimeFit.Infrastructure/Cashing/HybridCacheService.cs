using Microsoft.Extensions.Caching.Hybrid;
using PrimeFit.Application.ServicesContracts.Infrastructure.Cashing;

namespace PrimeFit.Infrastructure.Cashing
{
    public class HybridCacheService(HybridCache cache) : ICacheService
    {
        public async Task<T> GetOrCreateAsync<T>(
            string key,
            Func<CancellationToken, Task<T>> factory,
            TimeSpan? expiration = null,
            CancellationToken ct = default)
        {


            var options = new HybridCacheEntryOptions
            {
                Expiration = expiration ?? TimeSpan.FromMinutes(5)
            };


            return await cache.GetOrCreateAsync<object?, T>(
                key,
                state: null,
                async (_, token) =>
                    await factory(token),
                options,
                cancellationToken: ct
            );
        }

        public async Task RemoveAsync(string key, CancellationToken ct = default)
        {
            await cache.RemoveAsync(key, cancellationToken: ct);
        }
    }
}



