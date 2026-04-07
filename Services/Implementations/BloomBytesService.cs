using System.Text;
using GNS.Data.Entities;
using GNS.Data.Repositories.Interfaces;
using GNS.Services.Interfaces;

namespace GNS.Services.Implementations
{
    public class BloomBytesService : IBloomBytesService
    {
        private readonly IBloomBytesRepository _bloomBytesRepository;
        private readonly IUnitOfWork _unitOfWork;
        public BloomBytesService(
            IBloomBytesRepository bloomBytesRepository,
            IUnitOfWork unitOfWork)
        {
            _bloomBytesRepository = bloomBytesRepository;
            _unitOfWork = unitOfWork;
        }
        public byte[] GetBytes(string word)
        {
            if (word.Length < 7)
            {
                // Validation Error
                throw new Exception($"{word} must contain at least 7 chars");
            }

            var wordBytes = Encoding.UTF8.GetBytes(word);
            var result = new byte[4];

            for (int i = 0; i <= 3 ; i++)
            {
                result[i] = wordBytes[i * 2];
            }

            return result;
        }


        public async Task<bool> ContainsEmailDataAsync(string email, CancellationToken token = default)
        {
            var emailBytes = GetBytes(email);

            return await _bloomBytesRepository.AnyAsync(bb => bb.EmailBytes == emailBytes, token);
        }

        public async Task<bool> ContainsUserNameDataAsync(string userName, CancellationToken token = default)
        {
            var userNameBytes = GetBytes(userName);

            return await _bloomBytesRepository.AnyAsync(bb => bb.UserNameBytes == userNameBytes, token);
        }
        public async Task<Guid> SaveBloomBytesAsync(
            string email,
            string userName,
            CancellationToken token = default
            )
        {
            var emailBytes = GetBytes(email);
            var userNameBytes = GetBytes(userName);

            var bloomBytesEntity = new BloomBytesEntity
            {
                EmailBytes = emailBytes,
                UserNameBytes = userNameBytes
            };
            var bloomBytesId = bloomBytesEntity.Id;
            await _bloomBytesRepository.AddAsync(bloomBytesEntity, token);
            await _unitOfWork.SaveChangesAsync(token);

            return bloomBytesId;
        }
    }
}