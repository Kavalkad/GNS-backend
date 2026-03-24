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
                .HasMany(wh => wh.CyberClubs)
                .WithMany(cc => cc.WorkingHours)
                .UsingEntity<CyberClubWorkingHoursEntity>(
                    l => l.HasOne<CyberClubEntity>().WithMany().HasForeignKey(ccwh => ccwh.CyberClubId),
                    r => r.HasOne<WorkingHoursEntity>().WithMany().HasForeignKey(ccwh => ccwh.WorkingHoursId)
                );

               


        }
    }
}