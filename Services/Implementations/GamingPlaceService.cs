using GNS.Dto;
using GNS.Services.Interfaces;
using GNS.Contracts.Requests;
using GNS.Data.Repositories.Interfaces;
using GNS.Data.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using GNS.Enums;
using GNS.Extensions;
using GNS.Exceptions;
using GNS.Contracts.Requests.Implementations;


namespace GNS.Services.Implementations
{
    public class GamingPlaceService(
        IGamingPlacesRepository gamingPlacesRepository,
        IUnitOfWork unitOfWork,
        ICyberClubService cyberClubService,
        IMapper mapper
            ) : IGamingPlaceService
    {
        private readonly IGamingPlacesRepository _gamingPlacesRepository = gamingPlacesRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ICyberClubService _cyberClubService = cyberClubService;
        private readonly IMapper _mapper = mapper;

        public async Task AddGamingPlacesAsync(CreateGamingPlacesRequest request, CancellationToken token = default)
        {
            // Сделать GetWithDetails в репозитории, но не в BaseRepository

            var cyberClub = await _cyberClubService.GetClubByIdAsync(request.CyberClubId, token)
                ?? throw new EntityNotFoundException("Cyber club", request.CyberClubId.ToString());

            var maxGamingPlaceNumber = cyberClub.GamingPlaces.Count;
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

            await _gamingPlacesRepository.AddRangeAsync(gamingPlaces, token);
            await _unitOfWork.SaveChangesAsync(token);
        }
       
        public async Task<GamingPlaceEntity> GetByIdAsync(Guid gamingPlaceId, CancellationToken token = default)
        {
            return await _gamingPlacesRepository.GetByIdAsync(gamingPlaceId, token)
                ?? throw new EntityNotFoundException("gmaing place", $"gaming place id: {gamingPlaceId}");
        }

        public async Task<GamingPlaceEntity> GetByIdWithDetails(Guid gamingPlaceId, CancellationToken token = default)
        {
            return await _gamingPlacesRepository.GetByIdWithDetailsAsync(gamingPlaceId, token)
                 ?? throw new EntityNotFoundException("gmaing place", $"gaming place id: {gamingPlaceId}");
        }
        

        public async Task<List<GamingPlaceDto>> GetCCGamingPlacesAsync(Guid cyberClubId, CancellationToken token = default)
        {
            var gamingPlaces = await _gamingPlacesRepository.GetByExpressionAsync(gp => gp.CyberClubId == cyberClubId, token)
                ?? throw new EntityNotFoundException("Gaming place", $"cyberClubId: {cyberClubId}");

            var orderedGamingPlaces = gamingPlaces.OrderBy(gp => gp.Number);

            return _mapper.MapToGamingPlaceDto(orderedGamingPlaces);
        }
        public async Task UpdateGamingPlaceNumberAsync(UpdateGamingPlaceNumberRequest request, CancellationToken token = default)
        {
            var gamingPlace = await _gamingPlacesRepository.GetByIdAsync(request.GamingPlaceId, token)
                ?? throw new EntityNotFoundException("gaming place", request.GamingPlaceId.ToString());

            gamingPlace.Number = request.Number;

            _gamingPlacesRepository.Update(gamingPlace);
            await _unitOfWork.SaveChangesAsync(token);
        }
        public async Task UpdateGamingPlacePricePerHourAsync(UpdateGamingPlacePricePerHourRequest request, CancellationToken token = default)
        {
            var gamingPlace = await _gamingPlacesRepository.GetByIdAsync(request.GamingPlaceId, token)
                ?? throw new EntityNotFoundException("gaming place", request.GamingPlaceId.ToString());

            gamingPlace.PricePerHour = request.PricePerHour;
            
            _gamingPlacesRepository.Update(gamingPlace);
            await _unitOfWork.SaveChangesAsync(token);
        }
        public async Task DeleteGamingPlaceAsync(Guid gamingPlaceId, CancellationToken token = default)
        {
            var gamingPlace = await _gamingPlacesRepository.GetByIdAsync(gamingPlaceId, token)
                ?? throw new EntityNotFoundException("gaming place", gamingPlaceId.ToString());

            _gamingPlacesRepository.Delete(gamingPlace);
            await _unitOfWork.SaveChangesAsync(token);
        }
    }


}