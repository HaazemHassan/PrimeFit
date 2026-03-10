using Ardalis.Specification;
using PrimeFit.Domain.Common.Enums;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Features.Attendance.Commands.CreateCheckIn.Specs
{
    public class SubscriptionForCheckInSpec : Specification<Subscription>
    {
        public SubscriptionForCheckInSpec(int userId, int branchId)
        {
            Query.Where(s => s.UserId == userId && s.BranchId == branchId && s.Branch.BranchStatus == BranchStatus.Active)

                .OrderBy(s =>
                    s.Status == SubscriptionStatus.Active ? 0 :
                    s.Status == SubscriptionStatus.Frozen ? 1 :
                    s.Status == SubscriptionStatus.Scheduled ? 2 :
                    s.Status == SubscriptionStatus.Expired ? 3 :
                    4) //Canceled

                .Include(s => s.User)
                .Include(s => s.Package);
        }
    }
}
