using GNS.Dto;
using GNS.Services.Interfaces;
using GNS.Contracts.Requests;
using GNS.Data.Repositories.Interfaces;
using GNS.Data.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using GNS.Enums;
using GNS.Extensions;
using GNS.Exceptions;

namespace GNS.Services.Implementations
{
    public class GamingPlaceService : IGamingPlaceService
    {
        private readonly IGamingPlacesRepository _gamingPlacesRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICyberClubService _cyberClubService;


        public GamingPlaceService(
            IGamingPlacesRepository gamingPlacesRepository,
            IUnitOfWork unitOfWork,
            ICyberClubService cyberClubService
            )
        {
            _gamingPlacesRepository = gamingPlacesRepository;
            _unitOfWork = unitOfWork;
            _cyberClubService = cyberClubService;
        }
        public async Task AddGamingPlaces(AddGamingPlacesRequest request, CancellationToken token = default)
        {
            // Сделать GetWithDetails в репозитории, но не в BaseRepository

            var cyberClub = await _cyberClubService.GetClubByIdAsync(request.CyberClubId, token)
                ?? throw new EntityNotFoundException("Cyber club", request.CyberClubId.ToString());

            var maxGamingPlaceNumber = cyberClub.GamingPlacesCount;
            var gamingPlaces = new GamingPlaceEntity[request.Count];

            _ = Enum.TryParse(request.EquipmentName, out Equipment _equipment);

            for (int i = 0; i < request.Count; i++)
            {
                var gamingPlace = new GamingPlaceEntity
                {
                    Number = i + maxGamingPlaceNumber + 1,
                    PricePerHour = request.PricePerHour,
                    Equipment = _equipment,
                    CyberClubId = cyberClub.Id
                };
                gamingPlaces[i] = gamingPlace;
            }

            await _gamingPlacesRepository.AddRangeAsync(gamingPlaces);
            await _unitOfWork.SaveChangesAsync(token);
        }
        /*
        public async Task<List<GamingPlaceEntity>> GetByEquipment(Equipment equipment)
        {
            var ownerId = _contextAccessor.TryGetHttpUserId();

            return await _gamingPlacesRepository.GetByEquipmentAndOwnerId(ownerId: ownerId, equipment: equipment);
        }
        */

        public async Task<List<GamingPlaceDto>> GetCCGamingPlaces(Guid cyberClubId, CancellationToken token = default)
        {
            var gamingPlaces = await _gamingPlacesRepository.GetByExpressionAsync(gp => gp.CyberClubId == cyberClubId, token);

            return gamingPlaces
                .OrderBy(gp => gp.Number)
                .Select(gp => new GamingPlaceDto(gp))
                .ToList();
        }
        /*
        public async Task UpdateCCGamingPlaces(UpdateCCGamingPlacesRequest request)
        {
            await _gamingPlacesRepository.UpdateCCGamingPlaces(
                request.CyberClubName,
                request.NewCount,
                request.NewPricePerHour,
                request.NewEquipmentName);
        }
        */
        public async Task DeleteGamingPlaces(DeleteGamingPlacesRequest request, CancellationToken token = default)
        {
            _ = Enum.TryParse(request.EquipmentName, out Equipment equipment);

            if (!Guid.TryParse(request.CyberClubId, out Guid cyberClubId))
            {
                throw new IncorrectGuidException(request.CyberClubId);
            }
            var gamingPlaces = await _gamingPlacesRepository
                .GetByExpressionAsync(gp => gp.CyberClubId == cyberClubId && gp.Equipment == equipment);

            foreach (var gp in gamingPlaces)
            {
                _gamingPlacesRepository.Delete(gp);
            }
            await _unitOfWork.SaveChangesAsync(token);
        }
    }


}