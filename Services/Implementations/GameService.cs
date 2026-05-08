using GNS.Contracts.Requests;
using GNS.Data.Entities;
using GNS.Data.Repositories.Interfaces;
using GNS.Dto;
using GNS.Exceptions;
using GNS.Services.Interfaces;

namespace GNS.Services.Implementations
{
    public class GameService(
        IUnitOfWork unitOfWork,
        IGamesRepository gamesRepository,
        IMapper mapper) : IGameService
    {
        private readonly IGamesRepository _gamesRepository = gamesRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task AddAsync(CreateGameRequest request, CancellationToken token = default)
        {
            var gameEntity = new GameEntity
            {
                Title = request.Title,
                OnPc = request.OnPC,
                OnPlayStation = request.OnPlayStation,
                OnXbox = request.OnXbox
            };
            await _gamesRepository.AddAsync(gameEntity, token);
            await _unitOfWork.SaveChangesAsync(token);
        }

        public async Task<List<GameDto>> GetByTitleFilterAsync(string filter, CancellationToken token = default)
        {
            var games = await _gamesRepository.GetByExpressionAsync(g => g.Title.Contains(filter), token)
                ?? throw new EntityNotFoundException("games", filter);

            return _mapper.MapToGameDto(games);
        }
        public async Task UpdateTitleAsync(UpdateGameTitleRequest request, CancellationToken token = default)
        {
            var gameId = request.GameId;
            var game = await _gamesRepository.FindAsync(g => g.Id == gameId, token)
                ?? throw new EntityNotFoundException("Game", gameId.ToString());

            game.Title = request.NewTitle;

            _gamesRepository.Update(game);
            await _unitOfWork.SaveChangesAsync(token);
        }
        public async Task UpdateOnPCAsync(UpdateGameOnRequest request, CancellationToken token = default)
        {
            var gameId = request.GameId;
            var game = await _gamesRepository.FindAsync(g => g.Id == gameId, token)
                ?? throw new EntityNotFoundException("Game", gameId.ToString());

            game.OnPc = request.NewOnValue;

            _gamesRepository.Update(game);
            await _unitOfWork.SaveChangesAsync(token);
        }
        public async Task UpdateOnPlayStationAsync(UpdateGameOnRequest request, CancellationToken token = default)
        {
            var gameId = request.GameId;
            var game = await _gamesRepository.FindAsync(g => g.Id == gameId, token)
                ?? throw new EntityNotFoundException("Game", gameId.ToString());

            game.OnPlayStation = request.NewOnValue;

            _gamesRepository.Update(game);
            await _unitOfWork.SaveChangesAsync(token);
        }
        public async Task UpdateOnXboxAsync(UpdateGameOnRequest request, CancellationToken token = default)
        {
            var gameId = request.GameId;
            var game = await _gamesRepository.FindAsync(g => g.Id == gameId, token)
                ?? throw new EntityNotFoundException("Game", gameId.ToString());

            game.OnXbox = request.NewOnValue;

            _gamesRepository.Update(game);
            await _unitOfWork.SaveChangesAsync(token);
        }
        public async Task DeleteGameByIdAsync(Guid gameId, CancellationToken token = default)
        {
            await _gamesRepository.DeleteByIdAsync(gameId, token);
            await _unitOfWork.SaveChangesAsync(token);
        }
    }
}