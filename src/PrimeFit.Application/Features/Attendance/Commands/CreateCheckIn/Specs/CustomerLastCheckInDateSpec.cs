using Ardalis.Specification;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Features.Attendance.Commands.CreateCheckIn.Specs
{
    internal class CustomerLastCheckInDateSpec : Specification<CheckIn, DateTimeOffset>
    {
        public CustomerLastCheckInDateSpec(int customerId, int branchId)
        {

            Query.Where(c => c.CustomerId == customerId && c.BranchId == branchId)
                  .OrderByDescending(c => c.CreatedAt)
                  .Take(1)
                  .Select(c => c.CreatedAt);

        }
    }
}
