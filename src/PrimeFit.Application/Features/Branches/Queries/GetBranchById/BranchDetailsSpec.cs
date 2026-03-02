using Ardalis.Specification;
using PrimeFit.Application.Features.Branches.Queries.GetBranchById;
using PrimeFit.Application.Features.Branches.Shared.DTOS;
using PrimeFit.Domain.Common.Enums;
using PrimeFit.Domain.Entities;

public class BranchDetailsSpec : Specification<Branch, GetBranchByIdQueryResponse>
{
    public BranchDetailsSpec(int branchId, DateTimeOffset now)
    {
        var today = now.DayOfWeek;
        var yesterday = now.AddDays(-1).DayOfWeek;
        var timeNow = TimeOnly.FromTimeSpan(now.TimeOfDay);

        Query
            .Where(b => b.Id == branchId)
            .Select(b => new GetBranchByIdQueryResponse
            {
                Id = b.Id,
                Name = b.Name,
                Address = b.Address!,
                BranchType = b.BranchType,
                BranchStatus = b.BranchStatus,

                Governorate = b.Governorate == null ? null : new GovernorateDto
                {
                    Id = b.Governorate.Id,
                    Name = b.Governorate.Name
                },

                Images = b.Images.Select(i => new ImageDto
                {
                    Id = i.Id,
                    Url = i.Url,
                    Type = i.Type
                }).ToList(),

                IsOpenNow = b.WorkingHours.Any(wh =>
                    !wh.IsClosed &&
                    (
                        (wh.CloseTime > wh.OpenTime && wh.Day == today && timeNow >= wh.OpenTime && timeNow < wh.CloseTime) ||
                        (wh.CloseTime <= wh.OpenTime && wh.Day == today && timeNow >= wh.OpenTime) ||
                        (wh.CloseTime <= wh.OpenTime && wh.Day == yesterday && timeNow < wh.CloseTime)
                    )
                ),

                ActivePackagesCount = b.Packages.Count(p => p.IsActive),
                ActiveSubscriptionsCount = b.Subscriptions.Count(s => s.Status == SubscriptionStatus.Active),
            });
    }
}