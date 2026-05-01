using MediatR;

namespace PrimeFit.Application.Common.Idempotency
{

    public abstract class IdempotentCommand(Guid requestId) : IRequest, IIdempotentRequest
    {
        public Guid RequestId { get; } = requestId;
    }

    public abstract class IdempotentCommand<TResponse>(Guid requestId) : IRequest<TResponse>, IIdempotentRequest
    {
        public Guid RequestId { get; } = requestId;
    }

}
