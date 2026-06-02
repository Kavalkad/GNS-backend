using GNS.Interfaces;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using GNS.Services.Interfaces;
using GNS.Data.Entities;
using GNS.Data.Repositories.Interfaces;
using GNS.Exceptions;
using GNS.Contracts.Responses;
using Microsoft.AspNetCore.Authentication;
using GNS.Enums;


namespace GNS.Services.Implementations
{
    public class TokensService(
        IOptions<JwtOptions> jwtOptions,
        IOptions<RefreshTokenOptions> refreshTokenOptions,
        IRefreshTokensRepository refreshTokensRepository,
        IUnitOfWork unitOfWork,
        IClaimService claimService,
        IHttpContextAccessor contextAccessor
            ) : ITokenService
    {
        private readonly JwtOptions _jwtOptions = jwtOptions.Value;
        private readonly RefreshTokenOptions _refreshTokenOptions = refreshTokenOptions.Value;
        private readonly IRefreshTokensRepository _refreshTokensRepository = refreshTokensRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IClaimService _claimService = claimService;
        private readonly IHttpContextAccessor _contextAccessor = contextAccessor;

        public string GenerateAccessToken(Guid userId, Role userRole)
        {
            var claims = _claimService.GenerateClaims(userId, userRole);
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
        public async Task<VerifyRefreshTokenResponse> VerifyRefreshTokenAsync(string tokenValue, CancellationToken token = default)
        {
            if (!_contextAccessor.HttpContext!
                .Request.Cookies.TryGetValue("accessToken", out string accessToken))
            {
                throw new AuthenticationFailureException("Cannot find accessToken in cookies");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            tokenHandler.ValidateToken(accessToken, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_jwtOptions.SecretKey)),
                ValidateLifetime = false,
                ValidateIssuer = false,
                ValidateAudience = false,
            }, out SecurityToken validatedToken);

            var jwt = (JwtSecurityToken)validatedToken;
            var userIdClaim = jwt.Claims.FirstOrDefault(c => c.Type == "Id");

            if (userIdClaim is null)
            {
                Console.WriteLine("1111111111111");
                return new VerifyRefreshTokenResponse
                {
                    IsValid = false
                };
            }

            if (!Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                throw new IncorrectGuidException(userIdClaim.Value);
            }

            var userRefreshToken = await _refreshTokensRepository
                .GetWithDetailsAsync(rt => rt.UserId == userId, token);

            if (userRefreshToken is null)
            {
                Console.WriteLine("22222222222222222");
                return new VerifyRefreshTokenResponse
                {
                    IsValid = false
                };
            }


            if (!Guid.TryParse(tokenValue, out Guid guidRefreshTokenValue))
            {
                throw new IncorrectGuidException(tokenValue);
            }

            if (userRefreshToken.Token != guidRefreshTokenValue)
            {
                Console.WriteLine("333333333333333333333333");
                return new VerifyRefreshTokenResponse
                {
                    IsValid = false
                };
            }

            bool isValid = userRefreshToken.ExpiresAt > DateTime.Now;

            if (!isValid)
            {
                Console.WriteLine("4444444444444444444444");
                return new VerifyRefreshTokenResponse
                {
                    IsValid = false
                };
            }

            userRefreshToken.ExpiresAt = DateTime.Now.AddDays(_refreshTokenOptions.RefreshTokenValidityDays);
            userRefreshToken.Token = Guid.NewGuid();
            await _unitOfWork.SaveChangesAsync(token);

            var newAccessToken = GenerateAccessToken(userId, userRefreshToken.User.Role);

            return new VerifyRefreshTokenResponse
            {
                NewAccessToken = newAccessToken,
                IsValid = isValid
            };
        }
        public async Task<RefreshTokenEntity> GenerateRefreshTokenAsync(Guid userId, CancellationToken token = default)
        {
            var refreshToken = await _refreshTokensRepository.GetByUserIdAsync(userId, token);

            refreshToken ??= new RefreshTokenEntity
            {
                Token = Guid.NewGuid(),
                ExpiresAt = DateTime.Now.AddDays(_refreshTokenOptions.RefreshTokenValidityDays),
                UserId = userId
            };

            refreshToken.Token = Guid.NewGuid();
            refreshToken.ExpiresAt = DateTime.Now.AddDays(_refreshTokenOptions.RefreshTokenValidityDays);

            _refreshTokensRepository.Update(refreshToken);
            await _unitOfWork.SaveChangesAsync(token);

            return refreshToken;
        }
        public async Task<RefreshTokenEntity> GetByValueAsync(Guid refreshTokenValue, CancellationToken token = default)
        {
            return await _refreshTokensRepository.FindAsync(rt => rt.Token == refreshTokenValue, token)
                ?? throw new EntityNotFoundException("RefreshToken", refreshTokenValue.ToString());
        }

        public async Task<List<RefreshTokenEntity>> GetByUserIdAsync(Guid userId, CancellationToken token = default)
        {
            return await _refreshTokensRepository.GetByExpressionAsync(u => u.Id == userId, token)
                ?? throw new EntityNotFoundException("userid", userId.ToString());
        }


    }
}