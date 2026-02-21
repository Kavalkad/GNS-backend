using GNS.Dto;
using GNS.Services.Interfaces;
using GNS.Contracts.Requests;
using GNS.Data.Repositories.Interfaces;

namespace GNS.Services.Implementations
{
    public class GamingPlaceService : IGamingPlaceService
    {
        private readonly IGamingPlacesRepository _gamingPlacesRepository;
        public GamingPlaceService(IGamingPlacesRepository gamingPlacesRepository)
        {
            _gamingPlacesRepository = gamingPlacesRepository;
        }
        public async Task AddGamingPlaces(AddGamingPlacesRequest request)
        {
            await _gamingPlacesRepository.AddGamingPlaces(
                   request.CyberClubId,
                   request.Count,
                   request.PricePerHour,
                   request.EquipmentName);
        }
        
        public async Task<List<GamingPlaceDto>> GetCCGamingPlaces(Guid cyberClubId)
        {
            var gamingPlaces = await _gamingPlacesRepository.GetCCGamingPlaces(cyberClubId);


            return gamingPlaces
                .OrderBy(gp => gp.Number)
                .Select(gp => new GamingPlaceDto(gp))
                .ToList();
        }
        public async Task UpdateCCGamingPlaces(UpdateCCGamingPlacesRequest request)
        {
            await _gamingPlacesRepository.UpdateCCGamingPlaces(
                request.CyberClubName,
                request.NewCount,
                request.NewPricePerHour,
                request.NewEquipmentName);
        }
        public async Task DeleteCCGamingPlaces(DeleteCCGamingPlacesRequest request)
        {
            await _gamingPlacesRepository.DeleteCCGamingPlaces(
                request.CyberClubName,
                request.EquipmentName
            );
        }
    }

  
}