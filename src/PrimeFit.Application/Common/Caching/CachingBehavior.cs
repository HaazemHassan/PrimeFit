using ErrorOr;
using MediatR;
using PrimeFit.Application.Common.Cashing;
using PrimeFit.Application.ServicesContracts.Infrastructure.Cashing;
using System.Collections.Concurrent;
using System.Reflection;

namespace PrimeFit.Application.Common.Caching
{
    public class CachingBehavior<TRequest, TResponse>(
        ICacheService cacheService,
        IEnumerable<ICachePolicy<TRequest>> policies)
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {

        private static readonly ConcurrentDictionary<Type, MethodInfo> _methodCache = new();

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken ct)
        {
            var policy = policies.SingleOrDefault();

            if (policy is null || policy.ShouldSkipCache(request))
            {
                return await next(ct);

            }

            var responseType = typeof(TResponse);

            if (responseType.IsGenericType && responseType.GetGenericTypeDefinition() == typeof(ErrorOr<>))
            {
                var valueType = responseType.GetGenericArguments()[0];

                var method = GetHandleCacheMethod(valueType);

                var task = (Task<TResponse>)method.Invoke(this,
                [
                    policy,
                    request,
                    next,
                    ct
                ])!;

                return await task;
            }

            return await next(ct);
        }


        private async Task<TResponse> HandleCacheAsync<TValue>(
            ICachePolicy<TRequest> policy,
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken ct)
        {
            try
            {
                var cacheKey = policy.GetCacheKey(request);
                var tags = policy.GetCacheTags(request);

                var cachedValue = await cacheService.GetOrCreateAsync(
                    cacheKey,
                    async token =>
                    {
                        var response = await next(token);

                        var errorOr = (ErrorOr<TValue>)(object)response!;

                        if (errorOr.IsError)
                            throw new SkipCacheException<TValue>(errorOr);

                        return errorOr.Value;
                    },
                    policy.Expiration,
                    tags,
                    ct
                );

                ErrorOr<TValue> finalResult = cachedValue;

                return (TResponse)(object)finalResult;
            }
            catch (SkipCacheException<TValue> ex)
            {
                return (TResponse)(object)ex.Response;
            }
        }

        private MethodInfo GetHandleCacheMethod(Type valueType)
        {
            return _methodCache.GetOrAdd(valueType, t =>
                typeof(CachingBehavior<TRequest, TResponse>)
                    .GetMethod(nameof(HandleCacheAsync), BindingFlags.NonPublic | BindingFlags.Instance)!
                    .MakeGenericMethod(t));
        }
    }
}