using GNS.Contracts.Requests;

namespace GNS.Services.Interfaces
{
    public interface IOwnerService
    {
        Task RegisterOwner(RegisterOwnerRequest request);
    }
}