using GNS.Contracts;
using GNS.Data.Entities;

namespace GNS.Data.Repositories.Interfaces
{
    public interface ICyberClubsRepository
    {
        Task Add(Guid ownerId, string name, string city, string address);
        Task<List<CyberClubEntity>> GetAllClubs();
        Task<List<CyberClubEntity>> GetByCity(string city);
        Task<List<CyberClubEntity>> GetByOwnerId(Guid ownerId);
        Task Update(string name, string? newName, string? newCity, string? newAddress);
        Task DeleteById(Guid id);
        Task DeleteByName(string name);

    }
}