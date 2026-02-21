using GNS.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace GNS.Data.Configurations
{
    public class GameGamingPlaceConfiguration
        : IEntityTypeConfiguration<GameGamingPlaceEntity>
    {

        public void Configure(EntityTypeBuilder<GameGamingPlaceEntity> builder)
        {
            builder.HasKey(ggp => new { ggp.GameId, ggp.GamingPlaceId });       
        }
    }
}