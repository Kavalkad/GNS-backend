using GNS.Data.Entities;

namespace GNS.Data.Repositories.Interfaces
{
    public interface IGameGPsRepository
    {
        Task Create(GameEntity game, GamingPlaceEntity[] gamingPlaces);
    }
}