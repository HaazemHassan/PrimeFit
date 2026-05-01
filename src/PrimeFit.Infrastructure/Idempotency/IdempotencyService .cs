using Microsoft.EntityFrameworkCore;
using PrimeFit.Application.Common.Idempotency;
using PrimeFit.Infrastructure.Data;

namespace PrimeFit.Infrastructure.Idempotency
{
    internal sealed class IdempotencyService : IIdempotencyService
    {
        private readonly AppDbContext _context;

        public IdempotencyService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> RequestExistsAsync(Guid requestId, CancellationToken cancellationToken = default)
        {
            return await _context.Set<IdempotentRequest>()
                .AnyAsync(r => r.Id == requestId, cancellationToken);
        }

        public async Task CreateRequestAsync(Guid requestId, string name, CancellationToken cancellationToken = default)
        {
            var idempotentRequest = new IdempotentRequest
            {
                Id = requestId,
                Name = name
            };

            _context.Add(idempotentRequest);

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
