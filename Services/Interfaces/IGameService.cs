using GNS.Contracts;
using GNS.Contracts.Requests;
using GNS.Dto;

namespace GNS.Services.Interfaces
{
    public interface IGameService
    {
        Task Add(string title);
        Task<List<GameDto>> GetByFilter(string filter);
        Task Update(UpdateGameRequest request);
        Task Delete(Guid gameId);
    }
}