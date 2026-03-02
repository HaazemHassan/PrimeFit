using PrimeFit.Domain.Entities;
using System.Linq.Expressions;

namespace PrimeFit.Application.Specifications.Expressions
{
    public static class BranchExpressions
    {
        public static Expression<Func<Branch, bool>> IsOpenNow(
            DayOfWeek today,
            DayOfWeek yesterday,
            TimeOnly now)
        {
            return b => b.WorkingHours.Any(wh =>
                !wh.IsClosed &&
                (
                    (
                        wh.CloseTime > wh.OpenTime &&
                        wh.Day == today &&
                        now >= wh.OpenTime &&
                        now < wh.CloseTime
                    )
                    ||
                    (
                        wh.CloseTime <= wh.OpenTime &&
                        (
                            (wh.Day == today && now >= wh.OpenTime) ||
                            (wh.Day == yesterday && now < wh.CloseTime)
                        )
                    )
                )
            );
        }
    }
}
