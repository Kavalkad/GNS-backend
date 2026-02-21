using GNS.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GNS.Data.Configurations
{
    public class GamingPlaceConfiguration : IEntityTypeConfiguration<GamingPlaceEntity>
    {
        public void Configure(EntityTypeBuilder<GamingPlaceEntity> builder)
        {
            builder.HasKey(gp => gp.Id);


            builder
                .HasOne(gp => gp.CyberClub)
                .WithMany(cc => cc.GamingPlaces)
                .HasForeignKey(gp => gp.CyberClubId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasMany(gp => gp.Orders)
                .WithOne(o => o.GamingPlace)

                ;

            builder
                .HasMany(gp => gp.Games)
                .WithMany(g => g.GamingPlaces)
                .UsingEntity<GameGamingPlaceEntity>(
                    l => l.HasOne<GameEntity>().WithMany().HasForeignKey(ggp => ggp.GameId),
                    r => r.HasOne<GamingPlaceEntity>().WithMany().HasForeignKey(ggp => ggp.GamingPlaceId)
                );
        }
    }
}