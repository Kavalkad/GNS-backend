using GNS.Data.Entities;


namespace GNS.Data.Repositories.Interfaces
{
    public interface ICyberClubsRepository : IRepository<CyberClubEntity>
    {
        Task<CyberClubEntity?> GetWithDetailsAsync(Guid cyberClubId, CancellationToken token = default); 
    }
}