using GNS.Interfaces;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using GNS.Services.Interfaces;
using GNS.Data.Entities;
using GNS.Data.Repositories.Implementations;
using GNS.Data.Repositories.Interfaces;
using System.Threading.Tasks;


namespace GNS.Services.Implementations
{
    public class TokensService : ITokenService
    {
        private readonly JwtOptions _jwtOptions;
        private readonly IHasher _hasher;
        private readonly IRefreshTokensRepository _refreshTokensRepository;

        public TokensService(
            IOptions<JwtOptions> options,
            IHasher hasher,
            IRefreshTokensRepository refreshTokensRepository
            )
        {
            _jwtOptions = options.Value;
            _hasher = hasher;
            _refreshTokensRepository = refreshTokensRepository;
        }

        public string GenerateAccessToken(IClaimsGeneratable entity)
        {
            var claims = ClaimsBuilder.GenerateClaims(entity);
            var signinngCredentials = new SigningCredentials(
                                        new SymmetricSecurityKey(
                                           Encoding.UTF8.GetBytes(_jwtOptions.SecretKey)),
                                        SecurityAlgorithms.HmacSha256
                                        );
            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: signinngCredentials,
                expires: DateTime.UtcNow.AddMinutes(_jwtOptions.AccessTokenValidityMins)
            );

            var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenValue;

        }

        public async Task<RefreshTokenEntity> GenerateRefreshToken(Guid userId)
        {
            var refreshTokenValue = Guid.NewGuid();

            var refreshToken = new RefreshTokenEntity
            {
                Token = refreshTokenValue,
                ExpiresAt = DateTime.UtcNow.AddDays(_jwtOptions.RefreshTokenValidityDays),
                UserId = userId
            };

            await _refreshTokensRepository.AddAsync(refreshToken);
            return refreshToken;
        }
        public async Task<List<RefreshTokenEntity>> GetByUserId(Guid userId)
        {
            return await _refreshTokensRepository.GetTokensByUserId(userId);
        }
        public async Task RevokeRefreshToken(string refreshTokenValue)
        {
           await _refreshTokensRepository.UpdateRefreshToken(refreshTokenValue);
        }
    }
}