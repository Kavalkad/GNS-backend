using Microsoft.EntityFrameworkCore;
using GNS.Enums;
using GNS.Data.Repositories.Interfaces;
using GNS.Data.Entities;

namespace GNS.Data.Repositories.Implementations
{
    public class UsersRepository : IUsersRepository
    {
        private readonly AppDbContext _dbcontext;
        public UsersRepository(AppDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        public async Task AddAsync(string email, string hashedPassword, string userName)
        {

            var userEntity = new UserEntity()
            {
                Email = email,
                HashedPassword = hashedPassword,
                UserName = userName,
                Role = Role.User
            };
            await _dbcontext.Users.AddAsync(userEntity);
            await _dbcontext.SaveChangesAsync();

        }

        public async Task DeleteByIdAsync(Guid id)
        {
            await _dbcontext.Users
                .Where(u => u.Id == id)
                .ExecuteDeleteAsync();
        }
        public async Task<UserEntity?> GetByEmailAsync(string email)
        {
            return await _dbcontext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email)
                    ?? throw new Exception($"User with email: {email} not found");
        }
        public async Task<UserEntity?> GetByUserNameAsync(string userName)
        {
            return await _dbcontext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.UserName == userName)
                    ?? throw new Exception($"User witn Username {userName} not found");
        }
        public async Task<bool> ContainsEmail(string email)
        {
            return await _dbcontext.Users
                .AsNoTracking()
                .AnyAsync(u => u.Email == email);
        }
        public async Task<bool> ContainsUserName(string userName)
        {
            return await _dbcontext.Users
                .AsNoTracking()
                .AnyAsync(u => u.UserName == userName);
        }

    }
}