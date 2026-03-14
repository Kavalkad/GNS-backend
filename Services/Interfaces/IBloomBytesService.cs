using GNS.Data.Entities;

namespace GNS.Services.Interfaces
{
    public interface IBloomBytesService
    {
        Task SaveBloomBytesAsync(
            BloomBytesEntity bloomBytesEntity,
            CancellationToken token = default
            );
        byte[] GetBytes(string word);
        Task<bool> FindEmailData(string email);
        Task<bool> FindUserNameData(string userName);

    }
}