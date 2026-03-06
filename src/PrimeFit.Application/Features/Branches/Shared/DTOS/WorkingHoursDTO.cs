namespace PrimeFit.Application.Features.Branches.Shared.DTOS
{
    public class WorkingHoursDTO
    {
        public DayOfWeek Day { get; set; }
        public TimeOnly OpenTime { get; set; }
        public TimeOnly CloseTime { get; set; }
        public bool IsClosed { get; set; }
    }
}
