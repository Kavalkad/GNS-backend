using GNS.Contracts;
using GNS.Contracts.Requests;
using GNS.Contracts.Requests.Implementations;
using GNS.Data.Entities;
using GNS.Dto;
using GNS.Enums;

namespace GNS.Services.Interfaces
{
    public interface IGamingPlaceService
    {
        Task AddGamingPlacesAsync(CreateGamingPlacesRequest request, CancellationToken token = default);
        Task<GamingPlaceEntity> GetByIdAsync(Guid gamingPlaceId, CancellationToken token = default);
        Task<(GamingPlaceEntity, string)> GetWithCyberClubName(Guid gamingPlaceId, CancellationToken token = default); 
        Task<List<GamingPlaceDto>> GetCCGamingPlacesAsync(Guid cyberClubId, CancellationToken token = default);
        Task UpdateGamingPlacePricePerHourAsync(UpdateGamingPlacePricePerHourRequest request, CancellationToken token = default);
        Task UpdateGamingPlaceNumberAsync(UpdateGamingPlaceNumberRequest request, CancellationToken token = default);
        Task DeleteGamingPlaceAsync(Guid gamingPlaceId, CancellationToken token = default);

    }
}