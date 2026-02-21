using GNS.Data.Configurations;
using GNS.Data.Entities; 
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

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
        public DbSet<GameGamingPlaceEntity> GameGamingPlaces { get; set; }
        public DbSet<OrderEntity> Orders { get; set; }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<OwnerEntity> Owners { get; set; }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        }
    }
}