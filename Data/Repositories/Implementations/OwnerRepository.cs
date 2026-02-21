using GNS.Enums;
using Microsoft.EntityFrameworkCore;
using GNS.Data.Repositories.Interfaces;
using GNS.Data.Entities;

namespace GNS.Data.Repositories.Implementations
{
    public class OwnersRepository : IOwnersRepository
    {
        private readonly AppDbContext _dbcontext;

        public OwnersRepository(AppDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<bool> ContainsOwnerId(Guid ownerId)
        {
            return await _dbcontext.Owners
                .AsNoTracking()
                .Select(owner => owner.Id)
                .ContainsAsync(ownerId);

        }

        public async Task CreateOwner(
            string email,
            string hashedPassword,
            string userName,
            string hashedSercretWord)
        {
            await _dbcontext.Owners.AddAsync(new OwnerEntity
            {
                Email = email,
                HashedPassword = hashedPassword,
                UserName = userName,
                HashedSecretWord = hashedSercretWord,
                Role = Role.Owner
            });
            await _dbcontext.SaveChangesAsync();
        }
    }
}