using GNS.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GNS.Data.Configurations
{
    public class WorkingHoursConfiguration : IEntityTypeConfiguration<WorkingHoursEntity>
    {
        public void Configure(EntityTypeBuilder<WorkingHoursEntity> builder)
        {
            builder.HasKey(wh => wh.Id);

            builder
                .HasOne(wh => wh.CyberClub)
                .WithOne(cc => cc.WorkingHours);

               


        }
    }
}