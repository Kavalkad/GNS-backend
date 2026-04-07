using GNS.Contracts.Requests;
using GNS.Contracts.Responses;
using GNS.Data.Entities;
using GNS.Data.Repositories.Interfaces;
using GNS.Exceptions;
using GNS.Services.Interfaces;

namespace GNS.Services.Implementations
{
    public class OwnerService : IOwnerService
    {
        private readonly IOwnersRepository _ownersRepository;
        private readonly IHasher _hasher;
        private readonly ITokenService _tokenService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBloomBytesService _bloomBytesService;
        public OwnerService(
            IOwnersRepository ownersRepository,
            IHasher hasher,
            ITokenService tokenService,
            IUnitOfWork unitOfWork,
            IBloomBytesService bloomBytesService)
        {
            _ownersRepository = ownersRepository;
            _hasher = hasher;
            _tokenService = tokenService;
            _unitOfWork = unitOfWork;
            _bloomBytesService = bloomBytesService;
        }

        public async Task RegisterOwnerAsync(RegisterOwnerRequest request, CancellationToken token = default)
        {
            try
            {
                Console.WriteLine("await _unitOfWork.BeginTransactionAsync(token);");
                await _unitOfWork.BeginTransactionAsync(token);

                Console.WriteLine("var bloomBytesId = await _bloomBytesService.SaveBloomBytesAsyn");
                
                var bloomBytesId = await _bloomBytesService.SaveBloomBytesAsync(request.Email, request.UserName, token);
                await _unitOfWork.SaveChangesAsync(token);

                Console.WriteLine("var hashedPassword = _hasher.Generate(reques");

                var hashedPassword = _hasher.Generate(request.Password);
                var hashedSuperSecretWord = _hasher.Generate(request.SuperSecretWord);
                var taxIdentificationNumber = request.TaxIdentificationNumber;

                Console.WriteLine(bloomBytesId.ToString());
                var owner = new OwnerEntity(
                    email: request.Email,
                    hashedPassword: hashedPassword,
                    userName: request.UserName,
                    hashedSuperSecretWord: hashedSuperSecretWord,
                    role: Enums.Role.Owner,
                    taxIdentificationNumber: taxIdentificationNumber,
                    bloomBytesId: bloomBytesId
                );

                Console.WriteLine("owner created");
                await _ownersRepository.AddAsync(owner, token);

                Console.WriteLine("Addition owner to db succed");
                await _unitOfWork.SaveChangesAsync(token);
                Console.WriteLine("saving changes succed");
                await _unitOfWork.CommitTransactionAsync(token);

            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync(token);

            }
        }
        public async Task<LoginOwnerResponse> Login(LoginOwnerRequest request, CancellationToken token = default)
        {
            var owner = await _ownersRepository.FindAsync(o => o.Email == request.Email, token)
                ?? throw new EntityNotFoundException("Owner", request.Email);

            var result = _hasher.Verify(request.Password, owner.HashedPassword)
                && _hasher.Verify(request.SuperSecretWord, owner.HashedSuperSecretWord);

            if (!result)
            {
                throw new Exception("Wrong password or supersecret word");
            }

            var accessToken = _tokenService.GenerateAccessToken(owner);
            var refreshToken = await _tokenService.GenerateRefreshTokenAsync(owner.Id, token);

            return new LoginOwnerResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token.ToString()
            };
        }

    }
}