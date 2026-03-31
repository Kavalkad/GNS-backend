using Microsoft.EntityFrameworkCore;
using GNS.Enums;
using GNS.Data.Repositories.Interfaces;
using GNS.Data.Entities;

namespace GNS.Data.Repositories.Implementations
{
    public class UsersRepository(AppDbContext dbcontext) 
        : BaseRepository<UserEntity>(dbcontext), IUsersRepository
    {

    }
}