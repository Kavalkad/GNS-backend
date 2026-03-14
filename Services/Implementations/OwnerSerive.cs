using GNS.Contracts.Requests;
using GNS.Contracts.Responses;
using GNS.Data.Entities;
using GNS.Data.Repositories.Interfaces;
using GNS.Services.Interfaces;

namespace GNS.Services.Implementations
{
    public class OwnerService : IOwnerService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IOwnersRepository _ownersRepository;
        private readonly IHasher _hasher;
        private readonly ITokenService _tokenService;
        private readonly UnitOfWork _unitOfWork;
        private readonly IBloomBytesService _bloomBytesService;
        public OwnerService(
            IOwnersRepository ownersRepository,
            IUsersRepository userService,
            IHasher hasher,
            ITokenService tokenService,
            UnitOfWork unitOfWork,
            IBloomBytesService bloomBytesService)
        {
            _ownersRepository = ownersRepository;
            _usersRepository = userService;
            _hasher = hasher;
            _tokenService = tokenService;
            _unitOfWork = unitOfWork;
            _bloomBytesService = bloomBytesService;
        }

        public async Task RegisterOwner(RegisterOwnerRequest request)
        {
            // Впихнуть в фильтр!!!!!!!!!!!!!!!!!!!!!
            var isValidEmail = await _bloomBytesService.FindEmailData(request.Email);
            var isValidUserName = await _bloomBytesService.FindUserNameData(request.UserName);
            if (!isValidEmail || !isValidUserName)
            {
                Results.InternalServerError($"Somebody with email: {request.Email} or username: {request.UserName} already exists");
            }

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var bloomBytesEntity = new BloomBytesEntity
                {
                    EmailBytes = _bloomBytesService.GetBytes(request.Email),
                    UserNameBytes = _bloomBytesService.GetBytes(request.UserName),
                };

                var hashedPassword = _hasher.Generate(request.Password);
                var hashedSuperSecretWord = _hasher.Generate(request.SuperSecretWord);

                var owner = new OwnerEntity(
                    email: request.Email,
                    hashedPassword: hashedPassword,
                    userName: request.UserName,
                    hashedSuperSecretWord: hashedSuperSecretWord,
                    role: Enums.Role.Owner,
                    bloomBytesId: bloomBytesEntity.Id
                );
                
                await _ownersRepository.AddOwner(owner);
                await _bloomBytesService.SaveBloomBytesAsync(bloomBytesEntity);


                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
            }


        }
        public async Task<LoginOwnerResponse> Login(LoginOwnerRequest request)
        {
            var owner = await _ownersRepository.GetByEmail(request.Email);
            var result = _hasher.Verify(request.Password, owner.HashedPassword);

            if (!result)
            {
                throw new Exception("Wrong password");
            }
            var accessToken = _tokenService.GenerateAccessToken(owner);
            var refreshToken = await _tokenService.GenerateRefreshToken(owner.Id);

            return new LoginOwnerResponse
            {

                AccessToken = accessToken,
                RefreshToken = refreshToken.ToString()
            };
        }
    }
}