using GNS.Data.Entities;

namespace GNS.Data.Repositories.Interfaces
{
    public interface IUsersRepository
    {
        Task AddUserAsync(UserEntity userEntity, CancellationToken token = default);
        Task<UserEntity?> GetByIdAsync(Guid userId, CancellationToken token = default);
        Task<UserEntity?> GetByEmailAsync(string email, CancellationToken token = default);
        Task<UserEntity?> GetByUserNameAsync(string userName, CancellationToken token = default);
        Task DeleteByIdAsync(Guid id, CancellationToken token = default);
        Task<bool> ContainsEmail(string email);
        Task<bool> ContainsUserName(string userName);

    }
}