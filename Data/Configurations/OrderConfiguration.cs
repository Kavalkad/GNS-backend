using GNS.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GNS.Data.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<OrderEntity>
    {
        public void Configure(EntityTypeBuilder<OrderEntity> builder)
        {
            builder.HasKey(o => o.Id);


          
                

            builder
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.ClientNoAction);


            builder
                .HasOne(o => o.GamingPlace)
                .WithMany(gp => gp.Orders)
                .HasForeignKey(o => o.GamingPlaceId);
              

        }
    }
}