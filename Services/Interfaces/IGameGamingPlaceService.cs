using GNS.Contracts;
using GNS.Contracts.Requests;

namespace GNS.Services.Interfaces
{
    public interface IGameGamingPlaceService
    {
        Task Add(CreateGameGPsRequest request);
        //Task Delete(DeleteGameGPsRequest request);
       // Task Update(UpdateGameGPsRequest request);
    }
}