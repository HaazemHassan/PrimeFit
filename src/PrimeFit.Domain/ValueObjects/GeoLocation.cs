using ErrorOr;

namespace PrimeFit.Domain.ValueObjects
{
    public record GeoLocation
    {
        public double Latitude { get; }
        public double Longitude { get; }

        private GeoLocation(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public static ErrorOr<GeoLocation> Create(double latitude, double longitude)
        {
            if (latitude < -90 || latitude > 90)
                return Error.Validation(description: "Latitude must be between -90 and 90.");

            if (longitude < -180 || longitude > 180)
                return Error.Validation(description: "Longitude must be between -180 and 180.");

            return new GeoLocation(latitude, longitude);
        }
    }
}

