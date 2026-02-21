
using GNS.Data.Entities;

namespace GNS.Data.Repositories.Interfaces
{
    public interface IGamesRepository
    {
        Task Add(string title);
        Task<List<GameEntity>> GetByFilter(string filter);
        Task Update(Guid gameId, string newTitle);
        Task Delete(Guid gameId);
    }
}