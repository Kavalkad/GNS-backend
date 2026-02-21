using GNS.Contracts;
using GNS.Contracts.Requests;
using GNS.Dto;

namespace GNS.Services.Interfaces
{
    public interface IGamingPlaceService
    {
        Task AddGamingPlaces(AddGamingPlacesRequest request);
        Task<List<GamingPlaceDto>> GetCCGamingPlaces(Guid cyberClubId);
        Task UpdateCCGamingPlaces(UpdateCCGamingPlacesRequest request);
        Task DeleteCCGamingPlaces(DeleteCCGamingPlacesRequest request);
    }
}