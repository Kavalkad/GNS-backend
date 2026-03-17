using GNS.Contracts.Requests;
using GNS.Enums;
using GNS.Services.Interfaces;
using GNS.Data.Repositories.Interfaces;
using GNS.Data.Entities;
using GNS.Extensions;

namespace GNS.Services.Implementations
{
    public class GameGPService : IGameGPService
    {
        private readonly IGameGPsRepository _gameGPRepository;
        private readonly IGamingPlaceService _gamingPlaceService;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ICyberClubService _cyberClubService;
        private readonly IUnitOfWork _unitOfWork;
        public GameGPService(
            IGameGPsRepository gameGPRepository,
            IGamingPlaceService gamingPlaceService,
            IHttpContextAccessor contextAccessor,
            ICyberClubService cyberClubService,
            IUnitOfWork unitOfWork)
        {
            _gameGPRepository = gameGPRepository;
            _gamingPlaceService = gamingPlaceService;
            _contextAccessor = contextAccessor;
            _cyberClubService = cyberClubService;
            _unitOfWork = unitOfWork;
        }

        public async Task Add(AddGameGPsRequest request)
        {
            // Кароче тут надо влепить сервис верификации овнера
            var equipment = Enum.Parse<Equipment>(request.EquipmentName);
            var gamingPlaces = await _gamingPlaceService.GetByEquipment(equipment);
            var pairs = gamingPlaces
                .Select(gp => new GameGamingPlaceEntity
                {
                    GameId = request.GameId,
                    GamingPlaceId = gp.Id
                }
                )
                .ToArray();
            await _gameGPRepository.AddPairs(pairs);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task Delete(DeleteGameGPsRequest request)
        {
            // И тута тоже надо сервис верификации
            var equipment = Enum.Parse<Equipment>(request.EquipmentName);
            var gameGPs = await _gamingPlaceService.GetByEquipment(equipment);
            var gameGPsIds = gameGPs.Select(ggp => ggp.Id);
            await _gameGPRepository.DeletePairs(request.GameId, gameGPsIds);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}