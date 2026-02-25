namespace GNS.Data.Repositories.Interfaces
{
    public interface IBloomBytesRepository
    {
        Task AddBytes(byte[] emailBytes, byte[] userNameBytes);
        Task<bool> ContainsEmailBytes(byte[] emailBytes);
        Task<bool> ContainsUserNameBytes(byte[] userNameBytes);
        Task DeleteBytes(byte[] emailBytes, byte[] userNameBytes);
    }
}