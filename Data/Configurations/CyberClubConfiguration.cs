using GNS.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GNS.Data.Configurations
{
    public class CyberClubConfiguration : IEntityTypeConfiguration<CyberClubEntity>
    {
        public void Configure(EntityTypeBuilder<CyberClubEntity> builder)
        {
            builder.HasKey(cc => cc.Id);


            builder.Property(cc => cc.Name)
                .HasMaxLength(25)
                .IsRequired();
            builder.HasIndex(cc => cc.Name);

            builder.Property(cc => cc.City)
                .HasMaxLength(25)
                .IsRequired();

            builder.Property(cc => cc.Address)
                .HasMaxLength(256)
                .IsRequired();



            builder
                .HasMany(cc => cc.Employees)
                .WithOne(e => e.CyberClub);


            builder
                .HasMany(cc => cc.GamingPlaces)
                .WithOne(gp => gp.CyberClub);

            builder
                .HasOne(cc => cc.WorkingHours)
                .WithOne(wh => wh.CyberClub);

                
        }
    }
}