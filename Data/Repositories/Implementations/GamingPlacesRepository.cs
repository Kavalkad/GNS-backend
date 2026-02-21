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

        public async Task AddGamingPlaces(
            Guid cyberClubId,
            int count,
            decimal pricePerHour,
            string equipmentName)
        {
            var cyberClub = _dbcontext.CyberClubs
                .Include(cc => cc.GamingPlaces)
                .FirstOrDefault(cc => cc.Id == cyberClubId)
                    ?? throw new Exception($"Cyberclub with Id {cyberClubId} not found");

            var gamingPlaceNumber = cyberClub.GamingPlaces.Count + 1;
            var gamingPlaces = new GamingPlaceEntity[count];
            var equipment = Enum
                .Parse<Equipment>(equipmentName)
                ;

            for (int i = 0; i < count; i++, gamingPlaceNumber++)
            {
                gamingPlaces[i] = new GamingPlaceEntity
                {
                    CyberClubId = cyberClub.Id,
                    Number = gamingPlaceNumber,
                    PricePerHour = pricePerHour,
                    Equipment = equipment
                };
            }

            await _dbcontext.GamingPlaces.AddRangeAsync(gamingPlaces);
            await _dbcontext.SaveChangesAsync();
        }
        public async Task<GamingPlaceEntity> GetByIdWithCC(Guid gamingPlaceId)
        {
            return await _dbcontext.GamingPlaces
                .AsNoTracking()
                .Include(gp => gp.CyberClub)
                .FirstOrDefaultAsync(gp => gp.Id == gamingPlaceId)
                ?? throw new Exception("GamingPlace not found()");
        }
        public async Task<List<GamingPlaceEntity>> GetCCGamingPlaces(Guid cyberClubId)
        {
            return await _dbcontext.GamingPlaces
                .AsNoTracking()
                .Where(gp => gp.CyberClubId == cyberClubId)
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
        public async Task DeleteCCGamingPlaces(string cyberClubName, string equipmentName)
        {
            await _dbcontext.GamingPlaces
                .Where(gp => gp.CyberClub.Name == cyberClubName
                    && gp.Equipment == Enum.Parse<Equipment>(equipmentName))
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