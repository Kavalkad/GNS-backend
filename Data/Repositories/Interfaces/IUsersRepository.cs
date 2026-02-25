using GNS.Data.Entities;

namespace GNS.Data.Repositories.Interfaces
{
    public interface IUsersRepository
    {
        Task AddAsync(string email, string hashedPassword, string userName);
        Task<UserEntity?> GetByEmailAsync(string email);
        Task<UserEntity?> GetByUserNameAsync(string userName);
        Task DeleteByIdAsync(Guid id);
        Task<bool> ContainsEmail(string email);
        Task<bool> ContainsUserName(string userName);

    }
}