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
using GNS.Exceptions;


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

        public async Task<RefreshTokenEntity> GenerateRefreshTokenAsync(Guid userId, CancellationToken token = default)
        {
            var refreshTokenValue = Guid.NewGuid();

            var refreshToken = new RefreshTokenEntity
            {
                Token = refreshTokenValue,
                ExpiresAt = DateTime.UtcNow.AddDays(_jwtOptions.RefreshTokenValidityDays),
                UserId = userId
            };

            await _refreshTokensRepository.AddAsync(refreshToken, token);
            await _unitOfWork.SaveChangesAsync(token);

            return refreshToken;
        }

        public async Task<List<RefreshTokenEntity>> GetByUserIdAsync(Guid userId, CancellationToken token = default)
        {
            return await _refreshTokensRepository.GetByExpressionAsync(u => u.Id == userId);
        }

        public async Task RevokeRefreshTokenAsync(string refreshTokenValue, CancellationToken token = default)
        {
            if (!Guid.TryParse(refreshTokenValue, out Guid result))
            {
                throw new IncorrectGuidException(refreshTokenValue, refreshTokenValue);
            }

            var refreshToken = await _refreshTokensRepository.FindAsync(rt => rt.Token == result, token)
                ?? throw new EntityNotFoundException("RefreshToken", refreshTokenValue);

            refreshToken.RevokedAt = DateTime.UtcNow;

            _refreshTokensRepository.Update(refreshToken);
            await _unitOfWork.SaveChangesAsync(token);
        }


    }
}