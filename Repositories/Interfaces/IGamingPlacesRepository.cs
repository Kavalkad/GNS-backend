using GNS.Data.Entities;
using GNS.Enums;

namespace GNS.Data.Repositories.Interfaces
{
    public interface IGamingPlacesRepository
    {
        Task AddGamingPlaces(GamingPlaceEntity[] gamingPlaces);
        Task<List<GamingPlaceEntity>> GetCCGamingPlaces(Guid cyberClubId);
        Task<List<GamingPlaceEntity>> GetGamingPlacesWithOrdersByCCId(Guid cyberClubId);
        Task<GamingPlaceEntity> GetByIdWithCC(Guid gamingPlaceId);
        Task<List<GamingPlaceEntity>> GetByEquipmentAndOwnerId(Guid ownerId, Equipment equipment);
        Task UpdateCCGamingPlaces(
            string cyberClubName,
            int newCount,
            decimal newPricePerHour,
            string newEquipmentName
        );
        Task DeleteCCGamingPlaces(string cyberClubName, Equipment equipment);

    }
}