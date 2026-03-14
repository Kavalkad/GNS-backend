using GNS.Data.Entities;

namespace GNS.Data.Repositories.Interfaces
{
    public interface IOwnersRepository
    {
        Task AddOwner(OwnerEntity ownerEntity);
        Task<OwnerEntity> GetByEmail(string email);
        Task<bool> ContainsOwnerId(Guid ownerId);
    }
}