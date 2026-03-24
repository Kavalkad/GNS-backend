using GNS.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace GNS.Data.Configurations
{
    public class CyberClubWorkingHoursConfiguration
        : IEntityTypeConfiguration<CyberClubWorkingHoursEntity>
    {

        public void Configure(EntityTypeBuilder<CyberClubWorkingHoursEntity> builder)
        {
            builder.HasKey(cchw => new { cchw.CyberClubId, cchw.WorkingHoursId });       
        }
    }
}