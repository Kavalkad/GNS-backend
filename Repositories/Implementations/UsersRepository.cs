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
        
        public async Task AddUserAsync(UserEntity userEntity, CancellationToken token = default)
        {
            await _dbcontext.Users.AddAsync(userEntity, token);
        }
        public async Task DeleteByIdAsync(Guid id, CancellationToken token = default)
        {
            await _dbcontext.Users
                .Where(u => u.Id == id)
                .ExecuteDeleteAsync(token);
        }
        public async Task<UserEntity?> GetByIdAsync(Guid userId, CancellationToken token = default)
        {
            return await _dbcontext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == userId, token);
                    
        }
        public async Task<UserEntity?> GetByEmailAsync(string email, CancellationToken token = default)
        {
            return await _dbcontext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email, token)
                    ?? throw new Exception($"User with email: {email} not found");
        }
        public async Task<UserEntity?> GetByUserNameAsync(string userName, CancellationToken token = default)
        {
            return await _dbcontext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.UserName == userName, token)
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