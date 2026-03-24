using GNS.Contracts.Responses;

namespace GNS.Services.Interfaces
{
    public interface IAuthService
    {
        Task<string> GetNewAcessToken(Guid userId);
        Task<VerifyRefreshTokenResponse> VerifyRefreshToken(string tokenValue, Guid userId);
    }
}