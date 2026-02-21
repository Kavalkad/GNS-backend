using GNS.Contracts.Requests;
using GNS.Data.Repositories.Interfaces;
using GNS.Dto;
using GNS.Services.Interfaces;

namespace GNS.Services.Implementations
{
    public class GameService : IGameService
    {
        private readonly IGamesRepository _gamesRepository;

        public GameService(IGamesRepository gamesRepository)
        {
            _gamesRepository = gamesRepository;
        }

        public async Task Add(string title)
        {
            await _gamesRepository.Add(title);
        }

        public async Task<List<GameDto>> GetByFilter(string filter)
        {
            var games = await _gamesRepository.GetByFilter(filter);

            return games.Select(g => new GameDto(g))
                .OrderBy(g => g.Title)
                .ToList();
        }
        public async Task Update(UpdateGameRequest request)
        {
            await _gamesRepository.Update(request.GameId, request.NewTitle);
        }
        public async Task Delete(Guid gameId)
        {
            await _gamesRepository.Delete(gameId);
        }
    }
}