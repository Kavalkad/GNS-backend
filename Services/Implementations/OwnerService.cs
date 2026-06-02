using GNS.Contracts.Requests;
using GNS.Contracts.Responses;
using GNS.Data.Entities;
using GNS.Data.Repositories.Interfaces;
using GNS.Exceptions;
using GNS.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;

namespace GNS.Services.Implementations
{
    public class OwnerService(
        IOwnersRepository ownersRepository,
        IHasher hasher,
        ITokenService tokenService,
        IUnitOfWork unitOfWork,
        IBloomBytesService bloomBytesService) : IOwnerService
    {
        private readonly IOwnersRepository _ownersRepository = ownersRepository;
        private readonly IHasher _hasher = hasher;
        private readonly ITokenService _tokenService = tokenService;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IBloomBytesService _bloomBytesService = bloomBytesService;

        public async Task RegisterOwnerAsync(RegisterOwnerRequest request, CancellationToken token = default)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync(token);

                var bloomBytesId = await _bloomBytesService.SaveBloomBytesAsync(request.Email, request.UserName, token);
                await _unitOfWork.SaveChangesAsync(token);

                var hashedPassword = _hasher.Generate(request.Password);
                var hashedSuperSecretWord = _hasher.Generate(request.SuperSecretWord);
                var taxIdentificationNumber = request.TaxIdentificationNumber;

                
                var owner = new OwnerEntity(
                    email: request.Email,
                    hashedPassword: hashedPassword,
                    userName: request.UserName,
                    hashedSuperSecretWord: hashedSuperSecretWord,
                    role: Enums.Role.Owner,
                    taxIdentificationNumber: taxIdentificationNumber,
                    bloomBytesId: bloomBytesId
                );

                await _ownersRepository.AddAsync(owner, token);
                await _unitOfWork.SaveChangesAsync(token);

                await _unitOfWork.CommitTransactionAsync(token);

            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync(token);
                throw;
            }
        }
        public async Task<LoginOwnerResponse> LoginAsync(LoginOwnerRequest request, CancellationToken token = default)
        {
            var owner = await _ownersRepository.FindAsync(o => o.Email == request.Email, token)
                ?? throw new EntityNotFoundException("Owner", request.Email);

            var result = _hasher.Verify(request.Password, owner.HashedPassword)
                && _hasher.Verify(request.SuperSecretWord, owner.HashedSuperSecretWord);

            if (!result)
            {
                throw new AuthenticationFailureException("Wrong password or supersecret word");
            }

            var accessToken = _tokenService.GenerateAccessToken(owner.Id, owner.Role);

            
            var refreshToken = await _tokenService.GenerateRefreshTokenAsync(owner.Id, token);


            return new LoginOwnerResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token.ToString(),
                Email = owner.Email,
                UserName = owner.UserName,
                TaxIdentificationNumber = owner.TaxIdentificationNumber,
                Role = owner.Role
            };
        }

        public async Task<OwnerEntity> GetOwnerByIdAsync(Guid ownerId, CancellationToken token = default)
        {
            return await _ownersRepository.GetByIdAsync(ownerId, token)
                ?? throw new EntityNotFoundException("owner", ownerId.ToString());
        }
    }
}