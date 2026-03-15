using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PrimeFit.Domain.Entities;
using PrimeFit.Domain.Repositories;

namespace PrimeFit.Infrastructure.Data.Repositories
{
    internal class VerificationCodeRepository : GenericRepository<VerificationCode>, IVerificationCodeRepository
    {

        private readonly DbSet<VerificationCode> _verificationCodes;


        public VerificationCodeRepository(AppDbContext context, IMapper mapper) : base(context, mapper)
        {
            _verificationCodes = context.Set<VerificationCode>();
        }

    }
}