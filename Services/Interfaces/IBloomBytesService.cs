using GNS.Data.Entities;

namespace GNS.Services.Interfaces
{
    public interface IBloomBytesService
    {
        Task<Guid> SaveBloomBytesAsync(
            string email,
            string userName,
            CancellationToken token = default
            );

        Task<bool> ContainsEmailDataAsync(string email, CancellationToken token = default);
        Task<bool> ContainsUserNameDataAsync(string userName, CancellationToken token = default);

    }
}