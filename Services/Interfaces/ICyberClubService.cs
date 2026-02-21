using GNS.Contracts.Requests;
using GNS.Dto;

namespace GNS.Services.Interfaces
{
    public interface ICyberClubService
    {
        Task Add(AddCyberClubRequest request);

        Task<List<CyberClubDto>> GetAllClubs();
        Task<List<CyberClubDto>> GetByCity(string city);
        Task<List<CyberClubDto>> GetMyCyberClubs();
        Task Update(UpdateCyberClubRequest request);
        Task DeleteById(Guid id);
        Task DeleteByName(string name);
    }
}