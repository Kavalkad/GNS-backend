using GNS.Contracts.Requests;
using GNS.Contracts.Responses;

namespace GNS.Services.Interfaces
{
    public interface IOwnerService
    {
        Task RegisterOwner(RegisterOwnerRequest request);
        Task<LoginOwnerResponse> Login(LoginOwnerRequest request);
    }
}