using GNS.Contracts.Responses;
using GNS.Data.Repositories.Interfaces;
using GNS.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace GNS.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly ITokenService _tokenService;
        public AuthService(
            ITokenService tokenService
            )
        {
            _tokenService = tokenService; 
        }
        public async Task<VerifyRefreshTokenResponse> VerifyRefreshToken(string tokenValue, Guid userId)
        {
            var userTokens = await _tokenService.GetByUserId(userId);

            var token = userTokens.FirstOrDefault(t => t.Token.ToString() == tokenValue)
                ?? throw new Exception($"User doesn't have refreshToken with value: {tokenValue}");

            bool isValid = token.ExpiresAt > DateTime.UtcNow && !token.IsRevoked;
            if (!isValid)
            {
                return new VerifyRefreshTokenResponse
                {
                    IsValid = isValid
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