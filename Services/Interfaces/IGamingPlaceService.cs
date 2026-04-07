using GNS.Contracts;
using GNS.Contracts.Requests;
using GNS.Data.Entities;
using GNS.Dto;
using GNS.Enums;

namespace GNS.Services.Interfaces
{
    public interface IGamingPlaceService
    {
        Task AddGamingPlaces(AddGamingPlacesRequest request, CancellationToken token = default);
        Task<List<GamingPlaceDto>> GetCCGamingPlaces(Guid cyberClubId, CancellationToken token = default);
        //Task UpdateCCGamingPlaces(UpdateCCGamingPlacesRequest request);
        Task DeleteGamingPlaces(DeleteGamingPlacesRequest request, CancellationToken token = default);
        // Task<List<GamingPlaceEntity>> GetByEquipment(Equipment equipment);
    }
}