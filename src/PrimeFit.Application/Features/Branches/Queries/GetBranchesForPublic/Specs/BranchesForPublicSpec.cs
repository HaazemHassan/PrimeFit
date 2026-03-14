using Ardalis.Specification;
using PrimeFit.Application.Specifications;
using PrimeFit.Domain.Common.Enums;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Features.Branches.Queries.GetBranchesForPublic.Specs
{
    public class BranchesForPublicSpec : Specification<Branch, GetBranchesForPublicQueryResponse>
    {
        public BranchesForPublicSpec(int? pageNumber, int? pageSize, string? search)
        {
            Query.Where(b => b.BranchStatus == BranchStatus.Active);

            if (!string.IsNullOrEmpty(search))
            {
                Query.Where(b => b.Name.Contains(search));
            }

            Query.Select(b => new GetBranchesForPublicQueryResponse
            {
                Id = b.Id,
                Name = b.Name,
                BranchType = b.BranchType,
                LogoUrl = b.Images
                    .Where(i => i.Type == BranchImageType.Logo)
                    .Select(i => i.Url)
                    .FirstOrDefault()!,
                Governate = b.Governorate != null ? b.Governorate.Name : null!,
                Address = b.Address!,
                Latitude = b.Coordinates!.Y,
                Longitude = b.Coordinates!.X,
                DistanceInMeters = null,
                TotalRatings = b.Reviews.Count,
                AverageRating = Math.Round(b.Reviews.Average(r => (double?)r.Rating) ?? 0, 1)
            });

            Query.Paginate(pageNumber, pageSize);
            Query.AsNoTracking();
        }
    }
}
