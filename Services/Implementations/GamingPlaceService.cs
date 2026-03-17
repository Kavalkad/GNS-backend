using GNS.Dto;
using GNS.Services.Interfaces;
using GNS.Contracts.Requests;
using GNS.Data.Repositories.Interfaces;
using GNS.Data.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using GNS.Enums;
using GNS.Extensions;

namespace GNS.Services.Implementations
{
    public class GamingPlaceService : IGamingPlaceService
    {
        private readonly IGamingPlacesRepository _gamingPlacesRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICyberClubService _cyberClubService;
        private readonly IHttpContextAccessor _contextAccessor;

        public GamingPlaceService(
            IGamingPlacesRepository gamingPlacesRepository,
            IUnitOfWork unitOfWork,
            ICyberClubService cyberClubService,
            IHttpContextAccessor contextAccessor
            )
        {
            _gamingPlacesRepository = gamingPlacesRepository;
            _unitOfWork = unitOfWork;
            _cyberClubService = cyberClubService;
            _contextAccessor = contextAccessor;
        }
        public async Task AddGamingPlaces(AddGamingPlacesRequest request)
        {
            var cyberClub = await _cyberClubService.GetById(request.CyberClubId);

            if (cyberClub is null)
            {
                Results.InternalServerError("CyberClub not found");
                return;
            }

            var maxGamingPlaceNumber = cyberClub?.GamingPlacesCount;
            var gamingPlaces = new GamingPlaceEntity[request.Count];

            _ = Enum.TryParse(request.EquipmentName, out Equipment _equipment);

            for (int i = 0; i < request.Count; i++)
            {
                var gamingPlace = new GamingPlaceEntity
                {
                    Number = i + maxGamingPlaceNumber!.Value + 1,
                    PricePerHour = request.PricePerHour,
                    Equipment = _equipment,
                    CyberClubId = cyberClub!.Id
                };
                gamingPlaces[i] = gamingPlace;
            }
            
            await _gamingPlacesRepository.AddGamingPlaces(gamingPlaces);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task<List<GamingPlaceEntity>> GetByEquipment(Equipment equipment)
        {
            var ownerId = _contextAccessor.TryGetHttpUserId();
            
            return await _gamingPlacesRepository.GetByEquipmentAndOwnerId(ownerId: ownerId, equipment: equipment);
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
            var equipment = Enum.Parse<Equipment>(request.EquipmentName);

            await _gamingPlacesRepository.DeleteCCGamingPlaces(
                request.CyberClubName,
                equipment
            );
        }
    }


}