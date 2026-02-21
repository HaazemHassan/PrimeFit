using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Infrastructure.Data.EntitiesConfigurations.BranchEntities
{
    public class GovernorateConfiguration : IEntityTypeConfiguration<Governorate>
    {
        public void Configure(EntityTypeBuilder<Governorate> builder)
        {
            builder.Property(x => x.Name)
                   .HasMaxLength(30)
                   .IsRequired();

            builder.HasData(
                new Governorate(1, "Cairo"),
                new Governorate(2, "Giza"),
                new Governorate(3, "Alexandria"),
                new Governorate(4, "Dakahlia"),
                new Governorate(5, "Red Sea"),
                new Governorate(6, "Beheira"),
                new Governorate(7, "Fayoum"),
                new Governorate(8, "Gharbia"),
                new Governorate(9, "Ismailia"),
                new Governorate(10, "Menoufia"),
                new Governorate(11, "Minya"),
                new Governorate(12, "Qalyubia"),
                new Governorate(13, "New Valley"),
                new Governorate(14, "Suez"),
                new Governorate(15, "Aswan"),
                new Governorate(16, "Assiut"),
                new Governorate(17, "Beni Suef"),
                new Governorate(18, "Port Said"),
                new Governorate(19, "Damietta"),
                new Governorate(20, "Sharkia"),
                new Governorate(21, "South Sinai"),
                new Governorate(22, "Kafr El Sheikh"),
                new Governorate(23, "Matrouh"),
                new Governorate(24, "Luxor"),
                new Governorate(25, "Qena"),
                new Governorate(26, "North Sinai"),
                new Governorate(27, "Sohag")
            );
        }
    }
}
