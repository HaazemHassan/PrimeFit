using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PrimeFit.Domain.Entities;
using PrimeFit.Domain.Repositories;
using PrimeFit.Infrastructure.Data;

namespace PrimeFit.Infrastructure.Data.Repositories
{
    internal class PaymentTransactionRepository : GenericRepository<PaymentTransaction>, IPaymentTransactionRepository
    {
        private readonly DbSet<PaymentTransaction> _paymentTransactions;

        public PaymentTransactionRepository(AppDbContext context, IMapper mapper) : base(context, mapper)
        {
            _paymentTransactions = context.Set<PaymentTransaction>();
        }
    }
}
