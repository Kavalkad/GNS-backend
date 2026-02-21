using GNS.Contracts.Requests;
using GNS.Data.Repositories.Interfaces;
using GNS.Services.Interfaces;

namespace GNS.Services.Implementations
{
    public class OwnerService : IOwnerService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IOwnersRepository _ownersRepository;
        private readonly IHasher _hasher;
        public OwnerService(
            IOwnersRepository ownersRepository,
            IUsersRepository userService,
            IHasher hasher)
        {
            _ownersRepository = ownersRepository;
            _usersRepository = userService;
            _hasher = hasher;
        }

        public async Task RegisterOwner(RegisterOwnerRequest request)
        {
            var hashedPassword = _hasher.Generate(request.Password);
            var hashedSercretWord = _hasher.Generate(request.SecretWord);

            await _ownersRepository.CreateOwner(
                request.Email,
                hashedPassword,
                request.UserName,
                hashedSercretWord);
                
        }
    }
}