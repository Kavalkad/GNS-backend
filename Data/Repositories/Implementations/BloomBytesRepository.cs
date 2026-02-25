using GNS.Data.Entities;
using GNS.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GNS.Data.Repositories.Implementations
{
    public class BloomBytesRepository : IBloomBytesRepository
    {
        private readonly AppDbContext _dbcontext;

        public BloomBytesRepository(AppDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task AddBytes(byte[] emailBytes, byte[] userNameBytes)
        {
            var bloomBytes = new BloomBytesEntity
            {
                EmailBytes = emailBytes,
                UserNameBytes = userNameBytes
            };

            await _dbcontext.BloomBytes.AddAsync(bloomBytes);
            await _dbcontext.SaveChangesAsync();
        }

        public async Task<bool> ContainsEmailBytes(byte[] emailBytes)
        {
            return await _dbcontext.BloomBytes
                .AsNoTracking()
                .AnyAsync(bb => bb.EmailBytes == emailBytes);
        }
        
        public async Task<bool> ContainsUserNameBytes(byte[] userNameBytes)
        {
            return await _dbcontext.BloomBytes
                .AsNoTracking()
                .AnyAsync(bb => bb.UserNameBytes == userNameBytes);
        }

        public async Task DeleteBytes(byte[] emailBytes, byte[] userNameBytes)
        {
            await _dbcontext.BloomBytes
                .Where(bb => bb.EmailBytes == emailBytes
                    && bb.UserNameBytes == userNameBytes)
                .ExecuteDeleteAsync();
        }
    }
}