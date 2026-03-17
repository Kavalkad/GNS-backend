using GNS.Contracts;
using GNS.Contracts.Requests;

namespace GNS.Services.Interfaces
{
    public interface IGameGPService
    {
        Task Add(AddGameGPsRequest request);
        Task Delete(DeleteGameGPsRequest request);
       
    }
}