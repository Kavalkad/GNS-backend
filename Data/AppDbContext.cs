using GNS.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace GNS.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        public DbSet<CyberClubEntity> CyberClubs { get; set; }
        public DbSet<WorkingHoursEntity> WorkingHours { get; set; }
        public DbSet<EmployeeEntity> Employees { get; set; }
        public DbSet<GameEntity> Games { get; set; }
        public DbSet<GamingPlaceEntity> GamingPlaces { get; set; }
        public DbSet<OrderEntity> Orders { get; set; }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<OwnerEntity> Owners { get; set; }
        public DbSet<BloomBytesEntity> BloomBytes { get; set; }
        public DbSet<RefreshTokenEntity> RefreshTokens { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}