using GNS.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GNS.Data.Configurations
{
    public class GameConfiguration : IEntityTypeConfiguration<GameEntity>
    {
        public void Configure(EntityTypeBuilder<GameEntity> builder)
        {
            builder.HasKey(g => g.Id);



            builder
                .HasMany(g => g.GamingPlaces)
                .WithMany(gp => gp.Games)
                .UsingEntity<GameGamingPlaceEntity>(
                    l => l.HasOne<GamingPlaceEntity>().WithMany().HasForeignKey(ggp => ggp.GamingPlaceId),
                    r => r.HasOne<GameEntity>().WithMany().HasForeignKey(ggp => ggp.GameId)
                );
                
        }
    }
}