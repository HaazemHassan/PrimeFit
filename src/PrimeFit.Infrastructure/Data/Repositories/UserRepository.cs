using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PrimeFit.Domain.Entities;
using PrimeFit.Domain.Repositories;

namespace PrimeFit.Infrastructure.Data.Repositories
{
    internal class UserRepository : GenericRepository<DomainUser>, IUserRepository
    {

        private readonly DbSet<DomainUser> _users;


        public UserRepository(AppDbContext context, IMapper mapper) : base(context, mapper)
        {
            _users = context.Set<DomainUser>();
        }

    }
}
