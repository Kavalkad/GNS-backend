using GNS.Contracts.Requests;
using GNS.Contracts.Responses;
using GNS.Data.Entities;

namespace GNS.Services.Interfaces
{
    public interface IOwnerService
    {
        Task RegisterOwnerAsync(RegisterOwnerRequest request, CancellationToken token = default);
        Task<LoginOwnerResponse> LoginAsync(LoginOwnerRequest request, CancellationToken token = default);
        Task<OwnerEntity> GetOwnerByIdAsync(Guid ownerId, CancellationToken token = default);
    }
}