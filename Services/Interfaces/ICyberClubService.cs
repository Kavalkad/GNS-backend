using GNS.Contracts.Requests;
using GNS.Data.Entities;
using GNS.Dto;

namespace GNS.Services.Interfaces
{
    public interface ICyberClubService
    {
        Task AddAsync(AddCyberClubRequest request, CancellationToken token = default);
        Task<List<CyberClubDto>> GetAllClubsAsync(CancellationToken token = default);
        Task<CyberClubEntity> FindByCyberClubNameAsync(string cyberClubName, CancellationToken token = default);
        Task<CyberClubDto> GetClubByIdAsync(Guid cyberClubId, CancellationToken token = default);
        Task<List<CyberClubDto>> GetByCityAsync(string city, CancellationToken token = default);
        Task<List<CyberClubDto>> GetOwnerCyberClubsAsync(Guid ownerId, CancellationToken token = default);
        Task UpdateCyberClubNameAsync(UpdateCyberClubNameRequest request, CancellationToken token = default);
        Task UpdateCyberClubCityAsync(UpdateCyberClubCityRequest request, CancellationToken token = default);
        Task UpdateCyberClubAddressAsync(UpdateCyberClubAddressRequest request, CancellationToken token = default);
        Task DeleteClubByIdAsync(Guid cybetClubId, CancellationToken token = default);
    }
}