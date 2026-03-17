using GNS.Contracts.Requests;
using GNS.Contracts.Responses;
using GNS.Data.Entities;
using GNS.Data.Repositories.Interfaces;
using GNS.Enums;
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

        public async Task Register(RegisterUserRequest request)
        {

            //var isUniqueEmail = await _bloomBytesService.FindEmailData(request.Email);
            //var isUniqueUserName = await _bloomBytesService.FindUserNameData(request.UserName);

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var hashedPassword = _hasher.Generate(request.Password);

                var bloomBytesEntity = new BloomBytesEntity
                {
                    EmailBytes = _bloomBytesService.GetBytes(request.Email),
                    UserNameBytes = _bloomBytesService.GetBytes(request.UserName),

                };
                var userEntity = new UserEntity
                (
                    email: request.Email,
                    hashedPassword: hashedPassword,
                    userName: request.UserName,
                    bloomBytesId: bloomBytesEntity.Id
                );
                await _bloomBytesService.SaveBloomBytesAsync(bloomBytesEntity);
                await _usersRepository.AddUserAsync(userEntity);

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

            }
            catch (Exception e)
            {
                await _unitOfWork.RollbackTransactionAsync();
                Results.InternalServerError("Operation failed. Please retry again." + e.Message);
            }

        }




        public async Task<LoginUserResponse> Login(LoginUserRequest request)
        {

            var user = await _usersRepository.GetByEmailAsync(request.Email)
                ?? throw new Exception("Wrong email");

            var result = _hasher.Verify(request.Password, user.HashedPassword);

            if (!result)
            {
                throw new Exception("Wrong password");
            }
            
            var accessToken = _tokenService.GenerateAccessToken(user);
            var refreshToken = await _tokenService.GenerateRefreshToken(user.Id) ;

            return new LoginUserResponse
            {
                UserName = user.UserName,
                AccessToken = accessToken,
                RefreshToken = refreshToken.ToString()
            };
        }

        public async Task DeleteUser()
        {
            var userId = _contextAccessor.TryGetHttpUserId();

            await _usersRepository.DeleteByIdAsync(userId);
            await _unitOfWork.SaveChangesAsync();
        }


    }
}