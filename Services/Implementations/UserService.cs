using System.Linq.Expressions;
using GNS.Contracts.Requests;
using GNS.Contracts.Responses;
using GNS.Data.Entities;
using GNS.Data.Repositories.Interfaces;
using GNS.Enums;
using GNS.Exceptions;
using GNS.Extensions;
using GNS.Services.Interfaces;

namespace GNS.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IHasher _hasher;
        private readonly ITokenService _tokenService;
        private readonly IBloomBytesService _bloomBytesService;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IUnitOfWork _unitOfWork;



        public UserService(
            IUsersRepository usersRepository,
            IHasher hasher,
            ITokenService tokenService,
            IHttpContextAccessor contextAccessor,
            IBloomBytesService bloomBytesService,
            IUnitOfWork unitOfWork

        )
        {
            _usersRepository = usersRepository;
            _hasher = hasher;
            _tokenService = tokenService;
            _contextAccessor = contextAccessor;
            _bloomBytesService = bloomBytesService;
            _unitOfWork = unitOfWork;
        }
        public async Task<UserEntity?> FindUserAsync(
            Expression<Func<UserEntity, bool>> predicate,
            CancellationToken token = default
            )
        {
            return await _usersRepository.FindAsync(predicate, token);     
        }

        public async Task RegisterAsync(RegisterUserRequest request, CancellationToken token = default)
        {

            //var isUniqueEmail = await _bloomBytesService.FindEmailData(request.Email);
            //var isUniqueUserName = await _bloomBytesService.FindUserNameData(request.UserName);

            try
            {
                await _unitOfWork.BeginTransactionAsync(token);

                var hashedPassword = _hasher.Generate(request.Password);

                var bloomBytesId = await _bloomBytesService.SaveBloomBytesAsync(request.Email, request.UserName, token);

                var userEntity = new UserEntity
                (
                    email: request.Email,
                    hashedPassword: hashedPassword,
                    userName: request.UserName,
                    bloomBytesId: bloomBytesId
                );

                await _usersRepository.AddAsync(userEntity);

                await _unitOfWork.SaveChangesAsync(token);
                await _unitOfWork.CommitTransactionAsync(token);

            }
            catch (Exception e)
            {
                await _unitOfWork.RollbackTransactionAsync();
                Results.InternalServerError("Operation failed. Please retry again." + e.Message);
            }

        }

        public async Task<LoginUserResponse> LoginAsync(LoginUserRequest request, CancellationToken token = default)
        {

            var user = await _usersRepository.FindAsync(u => u.Email == request.Email, token)
                ?? throw new EntityNotFoundException("User", request.Email);

            var result = _hasher.Verify(request.Password, user.HashedPassword);

            if (!result)
            {
                throw new Exception("Wrong password");
            }
            
            var accessToken = _tokenService.GenerateAccessToken(user);
            var refreshToken = await _tokenService.GenerateRefreshTokenAsync(user.Id, token);

            return new LoginUserResponse
            {
                UserName = user.UserName,
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token.ToString()
            };
        }

        public async Task DeleteUserAsync(CancellationToken token = default)
        {
            var userId = _contextAccessor.TryGetHttpUserId();

            await _usersRepository.DeleteByIdAsync(userId, token);
            await _unitOfWork.SaveChangesAsync(token);
        }


    }
}