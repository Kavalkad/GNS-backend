using GNS.Data.Entities;
using GNS.Data.Repositories.Interfaces;
using GNS.Enums;
using GNS.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GNS.Data.Repositories.Implementations
{
    public class GamingPlacesRepository : IGamingPlacesRepository
    {
        private readonly AppDbContext _dbcontext;
        public GamingPlacesRepository(AppDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task AddGamingPlaces(GamingPlaceEntity[] gamingPlaces)
        {
            await _dbcontext.GamingPlaces.AddRangeAsync(gamingPlaces);
        }
        public async Task<GamingPlaceEntity> GetByIdWithCC(Guid gamingPlaceId)
        {
            return await _dbcontext.GamingPlaces
                .AsNoTracking()
                .Include(gp => gp.CyberClub)
                .FirstOrDefaultAsync(gp => gp.Id == gamingPlaceId)
                    ?? throw new Exception("GamingPlace not found");
        }
        public async Task<List<GamingPlaceEntity>> GetCCGamingPlaces(Guid cyberClubId)
        {
            return await _dbcontext.GamingPlaces
                .AsNoTracking()
                .Where(gp => gp.CyberClubId == cyberClubId)
                .ToListAsync();
        }
        public async Task<List<GamingPlaceEntity>> GetByEquipmentAndOwnerId(Guid ownerId, Equipment equipment)
        {
            return await _dbcontext.GamingPlaces
                .AsNoTracking()
                .Where(gp => gp.Equipment == equipment)
                .Include(gp => gp.CyberClub)
                .Where(gp => gp.CyberClub.OwnerId == ownerId)
                .ToListAsync();

        }
        public async Task UpdateCCGamingPlaces(
            string cyberClubName,
            int newCount,
            decimal newPricePerHour,
            string newEquipmentName
        )
        {
            var builder = new Microsoft.EntityFrameworkCore.Query.UpdateSettersBuilder();
        }
        public async Task DeleteCCGamingPlaces(string cyberClubName, Equipment equipment)
        {
            await _dbcontext.GamingPlaces
                .Where(gp => gp.CyberClub.Name == cyberClubName
                    && gp.Equipment == equipment)
                .ExecuteDeleteAsync();
        }

        public async Task<List<GamingPlaceEntity>> GetGamingPlacesWithOrdersByCCId(Guid cyberClubId)
        {
            return await _dbcontext.GamingPlaces
                 .AsNoTracking()
                 .Include(gp => gp.Orders)
                 .Where(gp => gp.CyberClubId == cyberClubId)
                 .ToListAsync();
        }
    }
}