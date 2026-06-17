using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PrimeFit.Domain.Entities;
using PrimeFit.Domain.Repositories;

namespace PrimeFit.Infrastructure.Data.Repositories
{
    internal class UserDeviceTokenRepository : GenericRepository<UserDeviceToken>, IUserDeviceTokenRepository
    {
        private readonly DbSet<UserDeviceToken> _userDeviceTokens;

        public UserDeviceTokenRepository(AppDbContext context, IMapper mapper) : base(context, mapper)
        {
            _userDeviceTokens = context.Set<UserDeviceToken>();
        }

        public async Task<UserDeviceToken?> GetByTokenAsync(string token, CancellationToken ct)
        {
            return await _userDeviceTokens.FirstOrDefaultAsync(t => t.Token == token, ct);
        }
    }
}
