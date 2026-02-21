using GNS.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GNS.Data.Configurations
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<EmployeeEntity>
    {
        public void Configure(EntityTypeBuilder<EmployeeEntity> builder)
        {

            builder.ToTable("Employees");
            builder
                .HasOne(e => e.CyberClub)
                .WithMany(cc => cc.Employees)
                .HasForeignKey(e => e.CyberClubId)
                .OnDelete(DeleteBehavior.SetNull);


        }
    }
}