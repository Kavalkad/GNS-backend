using GNS.Data.Entities;

namespace GNS.Data.Repositories.Interfaces
{
    public interface IUsersRepository
    {
        Task AddAsync(string email, string hashedPassword, string userName);

        Task DeleteByIdAsync(Guid id);

        Task<UserEntity?> GetByEmailAsync(string email);
        Task<UserEntity?> GetByUserNameAsync(string userName);


       
    }
}