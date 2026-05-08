using GNS.Data.Entities;
using GNS.Enums;

namespace GNS.Data.Repositories.Interfaces
{
    public interface IGamingPlacesRepository : IRepository<GamingPlaceEntity>
    {
        Task<GamingPlaceEntity?> GetByIdWithDetailsAsync(Guid gamingPlaceId, CancellationToken token = default);
    }
}