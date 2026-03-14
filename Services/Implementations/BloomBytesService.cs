using System.Text;
using GNS.Data.Entities;
using GNS.Data.Repositories.Interfaces;
using GNS.Services.Interfaces;

namespace GNS.Services.Implementations
{
    public class BloomBytesService : IBloomBytesService
    {
        private readonly IBloomBytesRepository _bloomBytesRepo;
        public BloomBytesService(IBloomBytesRepository bloomBytesRepo)
        {
            _bloomBytesRepo = bloomBytesRepo;
        }
        public byte[] GetBytes(string word)
        {
            /* if (word.Length < 7)
             {
                 throw new Exception($"{word} length must be greater than 6");
             }
             */
            var bytes = Encoding.UTF8.GetBytes(word);

            return bytes;
        }

    
        public async Task<bool> FindEmailData(string email)
        {
            var emailBytes = GetBytes(email);

            return await _bloomBytesRepo.ContainsEmailBytes(emailBytes);
        }

        public async Task<bool> FindUserNameData(string userName)
        {
            var userNameBytes = GetBytes(userName);

            return await _bloomBytesRepo.ContainsEmailBytes(userNameBytes);
        }
        public async Task SaveBloomBytesAsync(
            BloomBytesEntity bloomBytesEntity,
            CancellationToken token = default
            )
        {
            await _bloomBytesRepo.AddBloomBytesAsync(bloomBytesEntity);
        }
    }
}