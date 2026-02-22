using ErrorOr;
using PrimeFit.Domain.Common.Constants;
using PrimeFit.Domain.Entities.Base;

namespace PrimeFit.Domain.Entities
{
    public class BranchWorkingHour : AuditableEntity<int>
    {

        public const int MinimumShiftDurationInHours = 1;
        private BranchWorkingHour(DayOfWeek day, TimeOnly openTime, TimeOnly closeTime, bool isClosed, int branchId)
        {
            Day = day;
            OpenTime = openTime;
            CloseTime = closeTime;
            IsClosed = isClosed;
            BranchId = branchId;
        }

        public DayOfWeek Day { get; private set; }
        public TimeOnly OpenTime { get; private set; }
        public TimeOnly CloseTime { get; private set; }

        public bool IsClosed { get; private set; }

        public int BranchId { get; private set; }

        public Branch Branch { get; private set; } = null!;



        public static ErrorOr<BranchWorkingHour> Create(DayOfWeek day, TimeOnly? openTime, TimeOnly? closeTime, bool isClosed, int branchId)
        {

            if (openTime is null || closeTime is null)
            {

                if (isClosed)
                {
                    return new BranchWorkingHour(day, TimeOnly.MinValue, TimeOnly.MinValue, true, branchId);
                }

                return Error.Validation(ErrorCodes.WorkingHours.InvalidWorkingHours,
                    "Open and close time are required");
            }

            if (!HasMinimumShiftDuration(openTime.Value, closeTime.Value))
            {
                return Error.Validation(ErrorCodes.WorkingHours.ShiftDurationTooShort
                    , $"Shift duration must be at least {MinimumShiftDurationInHours} hours");
            }

            return new BranchWorkingHour(day, openTime.Value, closeTime.Value, false, branchId);
        }


        private static bool HasMinimumShiftDuration(TimeOnly openTime, TimeOnly closeTime)
        {
            var open = openTime.ToTimeSpan();
            var close = closeTime.ToTimeSpan();

            var duration = close > open
                ? close - open
                : close + TimeSpan.FromHours(24) - open;

            return duration >= TimeSpan.FromHours(MinimumShiftDurationInHours);
        }
    }
}
