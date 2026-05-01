namespace PrimeFit.Application.Common.Idempotency
{
    public interface IIdempotentRequest
    {
        Guid RequestId { get; }
    }
}
