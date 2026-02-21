using GNS.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GNS.Data.Configurations
{
    public class OwnerConfiguration : IEntityTypeConfiguration<OwnerEntity>
    {
        public void Configure(EntityTypeBuilder<OwnerEntity> builder)
        {
            builder
                .HasMany(o => o.CyberClubs)
                .WithOne(cc => cc.Owner)
                .HasForeignKey("OwnerId");
                
            builder.ToTable("Owners");
        }
    }
}