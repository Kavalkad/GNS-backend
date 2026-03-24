using GNS.Enums;
using Microsoft.EntityFrameworkCore;
using GNS.Data.Repositories.Interfaces;
using GNS.Data.Entities;
using System.Runtime.CompilerServices;

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

        public async Task AddOwner(OwnerEntity ownerEntity)
        {
            await _dbcontext.Owners.AddAsync(ownerEntity);
        }
        public async Task<OwnerEntity> GetByEmail(string email)
        {
            return await _dbcontext.Owners
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Email == email)
                    ?? throw new Exception($"Owner with email {email} not found");
        }
         public async Task<OwnerEntity?> GetById(Guid ownerId)
        {
            return await _dbcontext.Owners
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == ownerId);
        }
    }
}