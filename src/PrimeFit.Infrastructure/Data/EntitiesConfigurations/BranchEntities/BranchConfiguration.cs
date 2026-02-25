using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetTopologySuite.Geometries;
using PrimeFit.Domain.Entities;
using PrimeFit.Domain.ValueObjects;

namespace PrimeFit.Infrastructure.Data.EntitiesConfigurations.BranchEntities
{
    public class BranchConfiguration : IEntityTypeConfiguration<Branch>
    {
        public void Configure(EntityTypeBuilder<Branch> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.Email)
                .HasMaxLength(150);

            builder.Property(x => x.PhoneNumber)
                .HasMaxLength(20);

            builder.Property(x => x.Address)
                .HasMaxLength(500);

            builder.Property(x => x.BranchType)
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.BranchStatus)
                .HasConversion<string>()
                .HasMaxLength(50);


            builder.HasOne(b => b.Governorate)
            .WithMany()
            .HasForeignKey(b => b.GovernorateId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(b => b.Owner)
                .WithMany()
                .HasForeignKey(b => b.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(s => s.Location)
              .HasConversion(
               geo => new Point(geo.Longitude, geo.Latitude) { SRID = 4326 },
               point => GeoLocation.Create(point.Y, point.X).Value!
              )
              .HasColumnType("geography");

            builder.Ignore(b => b.MarketPlaceImages);

        }
    }
}
