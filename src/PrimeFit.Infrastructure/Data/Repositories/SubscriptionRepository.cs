using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PrimeFit.Domain.Entities;
using PrimeFit.Domain.Repositories;
using PrimeFit.Infrastructure.Data;

namespace PrimeFit.Infrastructure.Data.Repositories
{
    internal class SubscriptionRepository : GenericRepository<Subscription>, ISubscriptionRepository
    {
        private readonly DbSet<Subscription> _subscriptions;

        public SubscriptionRepository(AppDbContext context, IMapper mapper) : base(context, mapper)
        {
            _subscriptions = context.Set<Subscription>();
        }
    }
}
