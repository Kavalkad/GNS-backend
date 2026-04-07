using GNS.Contracts.Requests;
using GNS.Data.Entities;
using GNS.Data.Repositories.Interfaces;
using GNS.Dto;
using GNS.Exceptions;
using GNS.Services.Interfaces;

namespace GNS.Services.Implementations
{
    public class GameService : IGameService
    {
        private readonly IGamesRepository _gamesRepository;
        private readonly IUnitOfWork _unitOfWork;

        public GameService(
            IGamesRepository gamesRepository,
            IUnitOfWork unitOfWork)
        {
            _gamesRepository = gamesRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task AddAsync(AddGameRequest request, CancellationToken token = default)
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
            var games = await _gamesRepository.GetByExpressionAsync(g => g.Title.Contains(filter), token);

            return games.Select(g => new GameDto(g))
                .OrderBy(g => g.Title)
                .ToList();
        }
        public async Task UpdateTitleAsync(UpdateGameTitleRequest request, CancellationToken token = default)
        {
            if (!Guid.TryParse(request.GameId, out Guid gameId))
            {
                throw new IncorrectGuidException(request.GameId);
            }
            var game = await _gamesRepository.FindAsync(g => g.Id == gameId, token)
                ?? throw new EntityNotFoundException("Game", request.GameId);

            game.Title = request.NewTitle;

            _gamesRepository.Update(game);
            await _unitOfWork.SaveChangesAsync(token);
        }
        public async Task UpdateOnPCAsync(UpdateGameOnRequest request, CancellationToken token = default)
        {
            if (!Guid.TryParse(request.GameId, out Guid gameId))
            {
                throw new IncorrectGuidException(request.GameId);
            }
            var game = await _gamesRepository.FindAsync(g => g.Id == gameId, token)
                ?? throw new EntityNotFoundException("Game", request.GameId);

            game.OnPc = request.NewOnValue;

            _gamesRepository.Update(game);
            await _unitOfWork.SaveChangesAsync(token);
        }
        public async Task UpdateOnPlayStationAsync(UpdateGameOnRequest request, CancellationToken token = default)
        {
            if (!Guid.TryParse(request.GameId, out Guid gameId))
            {
                throw new IncorrectGuidException(request.GameId);
            }
            var game = await _gamesRepository.FindAsync(g => g.Id == gameId, token)
                ?? throw new EntityNotFoundException("Game", request.GameId);

            game.OnPlayStation = request.NewOnValue;

            _gamesRepository.Update(game);
            await _unitOfWork.SaveChangesAsync(token);
        }
        public async Task UpdateOnXboxAsync(UpdateGameOnRequest request, CancellationToken token = default)
        {
            if (!Guid.TryParse(request.GameId, out Guid gameId))
            {
                throw new IncorrectGuidException(request.GameId);
            }
            var game = await _gamesRepository.FindAsync(g => g.Id == gameId, token)
                ?? throw new EntityNotFoundException("Game", request.GameId);

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