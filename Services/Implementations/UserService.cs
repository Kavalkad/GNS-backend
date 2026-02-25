using GNS.Contracts.Requests;
using GNS.Data.Repositories.Interfaces;
using GNS.Extensions;
using GNS.Services.Interfaces;

namespace GNS.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IHasher _hasher;
        private readonly IJwtProvider _jwtProvider;
        private readonly IHttpContextAccessor _contextAccessor;



        public UserService(
            IUsersRepository usersRepository,
            IHasher hasher,
            IJwtProvider jwtProvider,
            IHttpContextAccessor contextAccessor

        )
        {
            _usersRepository = usersRepository;
            _hasher = hasher;
            _jwtProvider = jwtProvider;
            _contextAccessor = contextAccessor;
        }

        public async Task Register(RegisterUserRequest request)
        {
            var hashedPassword = _hasher.Generate(request.Password);

            await _usersRepository.AddAsync(
                request.Email,
                hashedPassword,
                request.UserName
            );
        }

        public async Task<string> Login(LoginUserRequest request)
        {

            var user = await _usersRepository.GetByEmailAsync(request.Email)
                ?? throw new Exception("Wrong email");

            var result = _hasher.Verify(request.Password, user.HashedPassword);

            if (!result)
            {
                throw new Exception("Wrong password");
            }
            var token = _jwtProvider.GenerateToken(user);

            return token;
        }
        public async Task DeleteUser()
        {
            var userId = _contextAccessor.GetHttpUserId();
            
            await _usersRepository.DeleteByIdAsync(userId);
        }
       
    }
}