using ErrorOr;
using MediatR;
using PrimeFit.Application.ServicesContracts.Infrastructure.Cashing;
using System.Reflection;

namespace PrimeFit.Application.Common.Cashing
{
    public class CachingBehavior<TRequest, TResponse>(
        ICacheService cacheService,
        IEnumerable<ICachePolicy<TRequest>> policies)
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
        {
            var policy = policies.SingleOrDefault();

            if (policy is null)
            {
                return await next();
            }

            var responseType = typeof(TResponse);

            if (responseType.IsGenericType && responseType.GetGenericTypeDefinition() == typeof(ErrorOr<>))
            {
                var valueType = responseType.GetGenericArguments()[0];

                var method = typeof(CachingBehavior<TRequest, TResponse>)
                    .GetMethod(nameof(HandleCacheAsync), BindingFlags.NonPublic | BindingFlags.Instance)!
                    .MakeGenericMethod(valueType);

                var task = (Task<TResponse>)method.Invoke(this, [policy.GetCacheKey(request), policy, next, ct])!;
                return await task;
            }

            return await next();
        }

        private async Task<TResponse> HandleCacheAsync<TValue>(
            string cacheKey,
            ICachePolicy<TRequest> policy,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken ct)
        {
            try
            {
                var cachedValue = await cacheService.GetOrCreateAsync(
                    cacheKey,
                    async token =>
                    {
                        var response = await next();

                        var errorOr = (ErrorOr<TValue>)(object)response!;

                        if (errorOr.IsError)
                        {
                            throw new SkipCacheException<TValue>(errorOr);
                        }

                        return errorOr.Value;
                    },
                    policy.Expiration,
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
    }
}