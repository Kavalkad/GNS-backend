using GNS.Contracts.Responses;

namespace GNS.Services.Interfaces
{
    public interface IAuthService
    {
        Task<VerifyRefreshTokenResponse> VerifyRefreshToken(string tokenValue, Guid userId);
    }
}