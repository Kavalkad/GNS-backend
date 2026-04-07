using GNS.Contracts;
using GNS.Contracts.Requests;
using GNS.Dto;

namespace GNS.Services.Interfaces
{
    public interface IGameService
    {
        Task AddAsync(AddGameRequest request, CancellationToken token = default);
        Task<List<GameDto>> GetByTitleFilterAsync(string filter, CancellationToken token = default);
        Task UpdateTitleAsync(UpdateGameTitleRequest request, CancellationToken token = default);
        Task UpdateOnPCAsync(UpdateGameOnRequest request, CancellationToken token = default);
        Task UpdateOnPlayStationAsync(UpdateGameOnRequest request, CancellationToken token = default);
        Task UpdateOnXboxAsync(UpdateGameOnRequest request, CancellationToken token = default);
        Task DeleteGameByIdAsync(Guid gameId, CancellationToken token = default);
    }
}