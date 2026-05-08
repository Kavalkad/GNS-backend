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
    public class UserService(
        IUsersRepository usersRepository,
        IHasher hasher,
        ITokenService tokenService,
        IHttpContextAccessor contextAccessor,
        IBloomBytesService bloomBytesService,
        IUnitOfWork unitOfWork
        ) : IUserService
    {
        private readonly IUsersRepository _usersRepository = usersRepository;
        private readonly IHasher _hasher = hasher;
        private readonly ITokenService _tokenService = tokenService;
        private readonly IBloomBytesService _bloomBytesService = bloomBytesService;
        private readonly IHttpContextAccessor _contextAccessor = contextAccessor;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<UserEntity> GetUserByIdAsync(
            Guid userId,
            CancellationToken token = default
            )
        {
            return await _usersRepository.GetByIdAsync(userId, token)
                ?? throw new EntityNotFoundException("user", userId.ToString());     
        }
        public async Task<UserEntity?> FindByExpression(Expression<Func<UserEntity, bool>> predicate, CancellationToken token = default)
        {
            return await _usersRepository.FindAsync(predicate, token);
                
        }

        public async Task RegisterAsync(RegisterUserRequest request, CancellationToken token = default)
        {
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

                await _usersRepository.AddAsync(userEntity, token);

                await _unitOfWork.SaveChangesAsync(token);
                await _unitOfWork.CommitTransactionAsync(token);

            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync(token);
                throw;
            }
            finally
            {
                await _unitOfWork.DisposeAsync();
            }
        }

        public async Task<LoginUserResponse> LoginAsync(LoginUserRequest request, CancellationToken token = default)
        {

            var user = await _usersRepository.FindAsync(u => u.Email == request.Email, token)
                ?? throw new EntityNotFoundException("User", request.Email);

            var result = _hasher.Verify(request.Password, user.HashedPassword);

            if (!result)
            {
                throw new UnauthorizedAccessException();
            }
            
            var accessToken = _tokenService.GenerateAccessToken(user);
            var refreshToken = await _tokenService.GenerateRefreshTokenAsync(user.Id, token);

            return new LoginUserResponse
            {
                UserName = user.UserName,
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token.ToString(),
                Role = user.Role
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