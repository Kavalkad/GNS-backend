using GNS.Contracts.Requests;
using GNS.Enums;
using GNS.Services.Interfaces;
using GNS.Data.Repositories.Interfaces;
using GNS.Data.Entities;

namespace GNS.Services.Implementations
{
    public class GameGamingPlaceService : IGameGamingPlaceService
    {
        private readonly IGameGPsRepository _gameGPRepository;
        private readonly IGamingPlaceService _gamingPlaceService;
        private readonly IUnitOfWork _unitOfWork;
        public GameGamingPlaceService(
            IGameGPsRepository gameGPRepository,
            IGamingPlaceService gamingPlaceService,
            IUnitOfWork unitOfWork)
        {
            _gameGPRepository = gameGPRepository;
            _gamingPlaceService = gamingPlaceService;
            _unitOfWork = unitOfWork;
        }
        public async Task Add(CreateGameGPsRequest request)
        {
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
    }
}