using GNS.Contracts.Responses;
using GNS.Data.Entities;
using GNS.Enums;
using GNS.Interfaces;


namespace GNS.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateAccessToken(Guid userId, Role userRole);
        Task<VerifyRefreshTokenResponse> VerifyRefreshTokenAsync(string tokenValue, CancellationToken token = default);
        Task<RefreshTokenEntity> GenerateRefreshTokenAsync(Guid userId, CancellationToken token = default);

    }
}