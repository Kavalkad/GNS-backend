using System.Security.Claims;
using GNS.Contracts.Responses;
using GNS.Data.Repositories.Interfaces;
using GNS.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace GNS.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly ITokenService _tokenService;
        private readonly IUsersRepository _usersRepository;
        public AuthService(
            ITokenService tokenService,
            IUsersRepository usersRepository
            )
        {
            _tokenService = tokenService;
            _usersRepository = usersRepository;
        }
        public async Task<string> GetNewAcessTokenAsync(Guid userId, CancellationToken token = default)
        {
            var user = await _usersRepository.GetByIdAsync(userId, token)
                ?? throw new Exception($"User with id: {userId} not found");

            return _tokenService.GenerateAccessToken(user);
        }
        public async Task<VerifyRefreshTokenResponse> VerifyRefreshTokenAsync(string tokenValue, Guid userId, CancellationToken token = default)
        {
            var userTokens = await _tokenService.GetByUserIdAsync(userId, token);

            var userToken = userTokens.FirstOrDefault(t => t.Token.ToString() == tokenValue)
                ?? throw new Exception($"User doesn't have refreshToken with value: {tokenValue}");

            bool isValid = userToken.ExpiresAt > DateTime.Now && !userToken.IsRevoked;

            if (!isValid)
            {
                return new VerifyRefreshTokenResponse
                {
                    IsValid = false
                };
            }

            await _tokenService.RevokeRefreshTokenAsync(tokenValue, token);

            var newRefreshToken = await _tokenService.GenerateRefreshTokenAsync(userId, token);

            return new VerifyRefreshTokenResponse
            {
                NewRefreshToken = newRefreshToken,
                IsValid = isValid
            };
        }

    }
}