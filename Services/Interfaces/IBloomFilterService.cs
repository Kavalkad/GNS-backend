namespace GNS.Services.Interfaces
{
    public interface IBloomFilterService
    {
        Task AddBytes(string email, string userName);
        Task<bool> FindEmailData(string email);
        Task<bool> FindUserNameData(string userName);
    }
}