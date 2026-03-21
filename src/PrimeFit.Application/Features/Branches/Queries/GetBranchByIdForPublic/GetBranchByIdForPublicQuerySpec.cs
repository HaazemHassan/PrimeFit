using Ardalis.Specification;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using PrimeFit.Application.Features.Branches.Shared.DTOS;
using PrimeFit.Domain.Common.Enums;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Features.Branches.Queries.GetBranchByIdForPublic
{
    public class GetBranchByIdForPublicQuerySpec : Specification<Branch, GetBranchByIdForPublicQueryResponse>
    {
        public GetBranchByIdForPublicQuerySpec(int branchId, double? latitude = null, double? longitude = null)
        {
            Point? userLocation = null;

            if (latitude.HasValue && longitude.HasValue)
            {
                var geometryFactory = NtsGeometryServices.Instance
                    .CreateGeometryFactory(srid: 4326);

                userLocation = geometryFactory
              .CreatePoint(new Coordinate(longitude.Value, latitude.Value));

            }


            Query.Where(b => b.Id == branchId && b.BranchStatus == BranchStatus.Active);



            Query.Select(b => new GetBranchByIdForPublicQueryResponse
            {
                Id = b.Id,
                Name = b.Name,
                BranchType = b.BranchType,
                TotalRatings = b.Reviews.Count(),
                AverageRating = b.Reviews.Any() ? Math.Round(b.Reviews.Average(r => (double)r.Rating), 1) : 0,

                DistanceInMeters = userLocation != null && b.Coordinates != null
                                ? Math.Round(b.Coordinates.Distance(userLocation)) : null,

                Images = b.Images.Where(i => i.Status == BranchImageStatus.Active).Select(i => new ImageDto
                {
                    Id = i.Id,
                    Url = i.Url,
                    Type = i.Type
                }).ToList(),


                Location = new LocationDto
                {
                    Governorate = new GovernorateDto
                    {
                        Id = b.Governorate!.Id,
                        Name = b.Governorate.Name
                    },

                    Address = b.Address!,

                    Coordinates = new CoordinatesDto
                    {
                        Latitude = b.Coordinates!.Y,
                        Longitude = b.Coordinates.X
                    }
                },

                WorkingHours = b.WorkingHours.Select(wh => new WorkingHoursDTO
                {
                    Day = wh.Day,
                    OpenTime = wh.OpenTime,
                    CloseTime = wh.CloseTime,
                    IsClosed = wh.IsClosed
                }).ToList(),


                Packages = b.Packages.Where(p => p.IsActive).Select(p => new PackageDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    DurationInMonths = p.DurationInMonths,
                }).ToList(),

            });
            Query.AsNoTracking();

        }
    }
}
