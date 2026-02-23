using ErrorOr;

namespace PrimeFit.Domain.ValueObjects
{
    public record GeoLocation
    {
        public Coordinate Coordinate { get; }

        private GeoLocation(Coordinate coordinate)
        {
            Coordinate = coordinate;
        }

        public static ErrorOr<GeoLocation> Create(double latitude, double longitude)
        {
            var coordinateResult = Coordinate.Create(latitude, longitude);

            if (coordinateResult.IsError)
                return coordinateResult.Errors;

            return new GeoLocation(coordinateResult.Value);
        }
    }
}
