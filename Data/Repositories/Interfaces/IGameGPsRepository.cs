using GNS.Data.Entities;

namespace GNS.Data.Repositories.Interfaces
{
    public interface IGameGPsRepository
    {
        Task AddPairs(params GameGamingPlaceEntity[] pairs);
        Task DeletePairs(Guid gameId, IEnumerable<Guid> gamingPlaceIds);
    }
}