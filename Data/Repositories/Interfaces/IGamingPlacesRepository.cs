using System.Linq.Expressions;
using GNS.Data.Entities;


namespace GNS.Data.Repositories.Interfaces
{
    public interface IGamingPlacesRepository : IRepository<GamingPlaceEntity>
    {
        Task<int> CountAsync(Expression<Func<GamingPlaceEntity, bool>> predicate, CancellationToken token = default);
        Task<GamingPlaceEntity?> GetWithDetailsAsync(Guid gamingPlaceId, CancellationToken token = default);
    }
}