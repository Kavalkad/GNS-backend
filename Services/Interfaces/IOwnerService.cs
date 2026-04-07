using GNS.Contracts.Requests;
using GNS.Contracts.Responses;

namespace GNS.Services.Interfaces
{
    public interface IOwnerService
    {
        Task RegisterOwnerAsync(RegisterOwnerRequest request, CancellationToken token = default);
        Task<LoginOwnerResponse> Login(LoginOwnerRequest request, CancellationToken token = default);
    }
}