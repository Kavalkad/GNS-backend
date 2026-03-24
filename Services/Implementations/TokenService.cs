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
        private readonly IRefreshTokensRepository _refreshTokensRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TokensService(
            IOptions<JwtOptions> options,
            IRefreshTokensRepository refreshTokensRepository,
            IUnitOfWork unitOfWork
            )
        {
            _jwtOptions = options.Value;
            _refreshTokensRepository = refreshTokensRepository;
            _unitOfWork = unitOfWork;
        }

        public string GenerateAccessToken(IClaimsGeneratable entity)
        {
            var claims = ClaimsBuilder.GenerateClaims(entity);
            var signingCredentials = new SigningCredentials(
                                        new SymmetricSecurityKey(
                                           Encoding.UTF8.GetBytes(_jwtOptions.SecretKey)),
                                        SecurityAlgorithms.HmacSha256
                                        );

            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: signingCredentials,
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
            await _unitOfWork.SaveChangesAsync();

            return refreshToken;
        }
        public async Task<List<RefreshTokenEntity>> GetByUserId(Guid userId)
        {
            return await _refreshTokensRepository.GetTokensByUserId(userId);
        }
        public async Task RevokeRefreshToken(string refreshTokenValue)
        {
            await _refreshTokensRepository.RevokeRefreshToken(refreshTokenValue);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}