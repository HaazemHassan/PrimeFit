using ErrorOr;
using MediatR;
using PrimeFit.Application.ServicesContracts.Infrastructure.Cashing;

namespace PrimeFit.Application.Common.Caching
{
    public class CacheInvalidationBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ICacheService cache;
        private readonly IEnumerable<ICacheInvalidationPolicy<TRequest>> policies;

        public CacheInvalidationBehavior(
            ICacheService cache,
            IEnumerable<ICacheInvalidationPolicy<TRequest>> policies)
        {
            this.cache = cache;
            this.policies = policies;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            var response = await next(cancellationToken);

            if (response is IErrorOr { IsError: true })
            {
                return response;

            }

            foreach (var policy in policies)
            {
                foreach (var tag in policy.GetTags(request))
                {
                    await cache.RemoveByTagAsync(tag, cancellationToken);

                }
            }


            return response;
        }
    }
}
