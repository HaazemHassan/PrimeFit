namespace PrimeFit.Application.Common.Idempotency
{
    public interface IIdempotencyService
    {
        Task<bool> RequestExistsAsync(Guid requestId, CancellationToken cancellationToken = default);

        Task CreateRequestAsync(Guid requestId, string name, CancellationToken cancellationToken = default);
    }
}
