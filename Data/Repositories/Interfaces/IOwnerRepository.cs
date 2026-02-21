namespace GNS.Data.Repositories.Interfaces
{
    public interface IOwnersRepository
    {
        Task CreateOwner(string email, string hashedPassword, string userName, string hashedSercretWord);
        Task<bool> ContainsOwnerId(Guid ownerId);
    }
}