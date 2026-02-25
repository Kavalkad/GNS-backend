using GNS.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GNS.Data.Configurations
{
    public class BloomBytesConfiguration : IEntityTypeConfiguration<BloomBytesEntity>
    {
        public void Configure(EntityTypeBuilder<BloomBytesEntity> builder)
        {
            builder.HasKey(bb => bb.Id);

            builder
                .HasMany(bb => bb.Users)
                .WithOne(u => u.BlomBytes)
                .HasForeignKey(u => u.BloomBytesId);

        }
    }
}
