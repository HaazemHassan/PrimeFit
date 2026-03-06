using Ardalis.Specification;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using PrimeFit.Application.Specifications;
using PrimeFit.Domain.Common.Enums;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Features.Branches.Queries.GetNearbyBranches
{
    public class NearbyBranchesSpec : Specification<Branch, GetNearbyBranchesQueryResponse>
    {
        public NearbyBranchesSpec(double latitude, double longitude, double radiusInMeters, int? pageNumber, int? pageSize, string? search)
        {
            var geometryFactory = NtsGeometryServices.Instance
                .CreateGeometryFactory(srid: 4326);

            var userLocation = geometryFactory
                .CreatePoint(new Coordinate(longitude, latitude));

            // Bounding Box calculation
            const double metersPerDegree = 111320d;

            var latDelta = radiusInMeters / metersPerDegree;
            var lonDelta = radiusInMeters / (metersPerDegree * Math.Cos(latitude * Math.PI / 180));

            var minLat = latitude - latDelta;
            var maxLat = latitude + latDelta;
            var minLon = longitude - lonDelta;
            var maxLon = longitude + lonDelta;

            Query
                .Where(b => b.Coordinates != null)
                .Where(b => b.BranchStatus == BranchStatus.Active)

                // Bounding box filter
                .Where(b =>
                    b.Coordinates!.Y >= minLat &&
                    b.Coordinates.Y <= maxLat &&
                    b.Coordinates.X >= minLon &&
                    b.Coordinates.X <= maxLon)

                // actual distance filter
                .Where(b => b.Coordinates!.IsWithinDistance(userLocation, radiusInMeters));

            if (!string.IsNullOrEmpty(search))
                Query.Where(b => b.Name.Contains(search));

            Query.OrderBy(b => b.Coordinates!.Distance(userLocation));

            Query.Select(b => new GetNearbyBranchesQueryResponse
            {
                Id = b.Id,
                Name = b.Name,
                LogoUrl = b.Images
                    .Where(i => i.Type == BranchImageType.Logo)
                    .Select(i => i.Url)
                    .FirstOrDefault()!,
                Governate = b.Governorate != null ? b.Governorate.Name : null!,
                Latitude = b.Coordinates!.Y,
                Longitude = b.Coordinates!.X,
                DistanceInMeters = Math.Round(b.Coordinates!.Distance(userLocation)),
                TotalRatings = b.Reviews.Count,
                AverageRating = Math.Round(b.Reviews.Average(r => (double?)r.Rating) ?? 0, 1)
            });

            Query.Paginate(pageNumber, pageSize);
            Query.AsNoTracking();
        }
    }
}