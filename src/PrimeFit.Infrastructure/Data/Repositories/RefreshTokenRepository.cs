using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PrimeFit.Domain.Contracts.Repositories;
using PrimeFit.Domain.Entities;
using PrimeFit.Infrastructure.Data;

namespace PrimeFit.Infrastructure.Data.Repositories
{
    internal class RefreshTokenRepository : GenericRepository<RefreshToken>, IRefreshTokenRepository
    {

        private readonly DbSet<RefreshToken> _refreshTokens;


        public RefreshTokenRepository(AppDbContext context, IMapper mapper) : base(context, mapper)
        {
            _refreshTokens = context.Set<RefreshToken>();
        }

        public async Task DeleteExpiredTokensAsync(DateTime cutoffDate)
        {

            await _refreshTokens
                .Where(x => (x.RevokationDate != null && x.RevokationDate <= cutoffDate) ||
                            (x.Expires <= cutoffDate))
                .ExecuteDeleteAsync();
        }
    }
}
