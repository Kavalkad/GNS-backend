using GNS.Data.Entities;

namespace GNS.Data.Repositories.Interfaces
{
    public interface IUsersRepository
    {
        Task AddUserAsync(UserEntity userEntity, CancellationToken token = default);

        Task<UserEntity?> GetByEmailAsync(string email);
        Task<UserEntity?> GetByUserNameAsync(string userName);
        Task DeleteByIdAsync(Guid id);
        Task<bool> ContainsEmail(string email);
        Task<bool> ContainsUserName(string userName);

    }
}