using PrimeFit.Domain.Entities.Base;

namespace PrimeFit.Domain.Entities
{
    public class BranchWorkingHour : AuditableEntity<int>
    {
        public BranchWorkingHour(DayOfWeek day, TimeOnly? openTime, TimeOnly? closeTime, bool isClosed, int branchId)
        {
            Day = day;
            OpenTime = openTime;
            CloseTime = closeTime;
            IsClosed = isClosed;
            BranchId = branchId;
        }

        public DayOfWeek Day { get; private set; }
        public TimeOnly? OpenTime { get; private set; }
        public TimeOnly? CloseTime { get; private set; }

        public bool IsClosed { get; private set; }

        public int BranchId { get; private set; }

        public Branch Branch { get; private set; } = null!;
    }
}
