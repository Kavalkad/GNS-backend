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
        public async Task<string> GetNewAcessToken(Guid userId)
        {
            var user = await _usersRepository.GetByIdAsync(userId)
                ?? throw new Exception($"User with id: {userId} not found");
            return _tokenService.GenerateAccessToken(user);

        }
        public async Task<VerifyRefreshTokenResponse> VerifyRefreshToken(string tokenValue, Guid userId)
        {
            var userTokens = await _tokenService.GetByUserId(userId);

            var token = userTokens.FirstOrDefault(t => t.Token.ToString() == tokenValue)
                ?? throw new Exception($"User doesn't have refreshToken with value: {tokenValue}");

            bool isValid = token.ExpiresAt > DateTime.Now && !token.IsRevoked;

            if (!isValid)
            {
                return new VerifyRefreshTokenResponse
                {
                    IsValid = false
                };
            }

            await _tokenService.RevokeRefreshToken(tokenValue);

            var newRefreshToken = await _tokenService.GenerateRefreshToken(userId);

            return new VerifyRefreshTokenResponse
            {
                NewRefreshToken = newRefreshToken,
                IsValid = isValid
            };
        }


       
    }
}