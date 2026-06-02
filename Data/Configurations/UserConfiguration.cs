using GNS.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GNS.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.UserName)
                .IsRequired()
                .HasMaxLength(15);
                

            builder
                .HasMany(u => u.Orders)
                .WithOne(o => o.User);

            builder
                .HasOne(u => u.RefreshToken)
                .WithOne(rt => rt.User);


            builder
                .HasOne(u => u.BloomBytes)
                .WithMany(bb => bb.Users);

            builder.ToTable("Users");
        }
    }
}