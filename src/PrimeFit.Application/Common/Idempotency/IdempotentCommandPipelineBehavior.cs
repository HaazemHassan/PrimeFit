using ErrorOr;
using MediatR;

namespace PrimeFit.Application.Common.Idempotency;

internal sealed class IdempotentCommandPipelineBehavior<TRequest, TResponse>(
    IIdempotencyService idempotencyService)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IIdempotentRequest
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var exists = await idempotencyService.RequestExistsAsync(request.RequestId, cancellationToken);

        if (exists)
        {
            return (dynamic)Error.Conflict(
                         code: "Idempotency.DuplicateRequest",
                         description: "This request was already processed.");
        }


        await idempotencyService.CreateRequestAsync(request.RequestId, typeof(TRequest).Name, cancellationToken);


        var response = await next();


        return response;
    }
}