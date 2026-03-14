using GNS.Contracts;
using GNS.Contracts.Requests;
using GNS.Data.Entities;
using GNS.Dto;
using GNS.Enums;

namespace GNS.Services.Interfaces
{
    public interface IGamingPlaceService
    {
        Task AddGamingPlaces(AddGamingPlacesRequest request);
        Task<List<GamingPlaceDto>> GetCCGamingPlaces(Guid cyberClubId);
        Task UpdateCCGamingPlaces(UpdateCCGamingPlacesRequest request);
        Task DeleteCCGamingPlaces(DeleteCCGamingPlacesRequest request);
        Task<List<GamingPlaceEntity>> GetByEquipment(Equipment equipment);
    }
}