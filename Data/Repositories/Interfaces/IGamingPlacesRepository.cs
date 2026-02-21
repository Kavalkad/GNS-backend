using GNS.Data.Entities;

namespace GNS.Data.Repositories.Interfaces
{
    public interface IGamingPlacesRepository
    {
        Task AddGamingPlaces(
            Guid cyberClubId,
            int count,
            decimal pricePerHour,
            string equipmentName
            );
        Task<List<GamingPlaceEntity>> GetCCGamingPlaces(Guid cyberClubId);
        Task<List<GamingPlaceEntity>> GetGamingPlacesWithOrdersByCCId(Guid cyberClubId);
        Task<GamingPlaceEntity> GetByIdWithCC(Guid gamingPlaceId);
        Task UpdateCCGamingPlaces(
            string cyberClubName,
            int newCount,
            decimal newPricePerHour,
            string newEquipmentName
        );
        Task DeleteCCGamingPlaces(
                string cyberClubName,
                string equipmentName
            );
    }
}