using GNS.Contracts.Requests;
using GNS.Data.Entities;
using GNS.Dto;

namespace GNS.Services.Interfaces
{
    public interface ICyberClubService
    {
        Task Add(AddCyberClubRequest request);
        Task<bool> VerifyOwner(Guid ownerId, string cyberClubName);
        Task<List<CyberClubDto>> GetAllClubs();
        Task<CyberClubEntity> GetByCCName(string cyberClubName);
        Task<CyberClubDto> GetById(Guid cyberClubId);
        Task<List<CyberClubDto>> GetByCity(string city);
        Task<List<CyberClubDto>> GetMyCyberClubs();
        Task Update(UpdateCyberClubRequest request);
        Task DeleteById(Guid id);
        Task DeleteByName(string name);
    }
}