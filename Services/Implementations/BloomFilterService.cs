using GNS.Data.Repositories.Interfaces;
using GNS.Services.Interfaces;

namespace GNS.Services.Implementations
{
    public class BloomFilterService : IBloomFilterService
    {
        private readonly IBloomBytesRepository _bloomBytes;
        public BloomFilterService(IBloomBytesRepository bloomBytes)
        {
            _bloomBytes = bloomBytes;
        }
        private byte[] GetBytes(string word)
        {
            if (word.Length < 7)
            {
                throw new Exception($"{word} length must be greater than 7");
            }
            var bytes = new byte[4]{
                (byte)char.GetNumericValue(word[0]),
                (byte)char.GetNumericValue(word[2]),
                (byte)char.GetNumericValue(word[4]),
                (byte)char.GetNumericValue(word[6])
            };
            return bytes;
        }
        public async Task AddBytes(string email, string userName)
        {
            var emailBytes = GetBytes(email);
            var userNameBytes = GetBytes(userName);
            
            await _bloomBytes.AddBytes(emailBytes, userNameBytes);
        }
        public async Task<bool> FindEmailData(string email)
        {
            var emailBytes = GetBytes(email);
            
            return await _bloomBytes.ContainsEmailBytes(emailBytes);
        }

        public async Task<bool> FindUserNameData(string userName)
        {
            var userNameBytes = GetBytes(userName);

            return await _bloomBytes.ContainsEmailBytes(userNameBytes);
        }
    }
}