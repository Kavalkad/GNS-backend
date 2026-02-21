using GNS.Data.Entities;
using GNS.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GNS.Data.Repositories.Implementations
{
    public class CyberClubsRepository : ICyberClubsRepository
    {
        private readonly AppDbContext _dbcontext;
        public CyberClubsRepository(AppDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        public async Task Add(Guid ownerId, string name, string city, string address)
        {
            var cyberClub = new CyberClubEntity
            {
                OwnerId = ownerId,
                Name = name,
                City = city,
                Address = address
            };

            await _dbcontext.CyberClubs.AddAsync(cyberClub);
            await _dbcontext.SaveChangesAsync();
        }
        public async Task<List<CyberClubEntity>> GetAllClubs()
        {
            return await _dbcontext.CyberClubs
                .AsNoTracking()
                .Include(cc => cc.Employees)
                .ToListAsync();
        }

        public async Task<List<CyberClubEntity>> GetByPage(int page, int pageSize)
        {
            return await _dbcontext.CyberClubs
                .AsNoTracking()
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }


        public async Task<CyberClubEntity?> GetById(Guid id)
        {
            return await _dbcontext.CyberClubs
                .AsNoTracking()
                .FirstOrDefaultAsync(cc => cc.Id == id);
        }
        public async Task<List<CyberClubEntity>> GetByCity(string city)
        {
            var query = _dbcontext.CyberClubs.AsNoTracking();

            if (!string.IsNullOrEmpty(city))
            {
                query = query.Where(cc => cc.City.Contains(city));
            }
            return await query.ToListAsync();
        }
        public async Task<List<CyberClubEntity>> GetByOwnerId(Guid ownerId)
        {
            return await _dbcontext.CyberClubs
                .AsNoTracking()
                .Where(cc => cc.OwnerId == ownerId)
                .ToListAsync();
        }

        // Update
        public async Task Update(string name, string? newName, string? newCity, string? newAddress)
        {

            await _dbcontext.CyberClubs.Where(cc => cc.Name == name)
                .ExecuteUpdateAsync(cc =>
                    {
                        if (!string.IsNullOrEmpty(newName))
                        {
                            cc.SetProperty(cc => cc.Name, newName);
                        }
                        if (!string.IsNullOrEmpty(newCity))
                        {
                            cc.SetProperty(cc => cc.City, newCity);
                        }
                        if (!string.IsNullOrEmpty(newAddress))
                        {
                            cc.SetProperty(cc => cc.Address, newAddress);
                        }

                    }
                );
        }

        // Delete

        public async Task DeleteById(Guid id)
        {
            await _dbcontext.CyberClubs.Where(cc => cc.Id == id)
                .ExecuteDeleteAsync();

        }
        public async Task DeleteByName(string name)
        {
            await _dbcontext.CyberClubs.Where(cc => cc.Name == name)
                .ExecuteDeleteAsync();

        }

       
    }
}