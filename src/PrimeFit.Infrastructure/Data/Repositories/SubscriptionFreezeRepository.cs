using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PrimeFit.Domain.Entities;
using PrimeFit.Domain.Repositories;
using PrimeFit.Infrastructure.Data;

namespace PrimeFit.Infrastructure.Data.Repositories
{
    internal class SubscriptionFreezeRepository : GenericRepository<SubscriptionFreeze>, ISubscriptionFreezeRepository
    {
        private readonly DbSet<SubscriptionFreeze> _subscriptionFreezes;

        public SubscriptionFreezeRepository(AppDbContext context, IMapper mapper) : base(context, mapper)
        {
            _subscriptionFreezes = context.Set<SubscriptionFreeze>();
        }
    }
}
