using GNS.Contracts.Responses;

namespace GNS.Services.Interfaces
{
    public interface IAuthService
    {
        Task<string> GetNewAcessTokenAsync(Guid userId, CancellationToken token = default);
        Task<VerifyRefreshTokenResponse> VerifyRefreshTokenAsync(string tokenValue, Guid userId, CancellationToken token = default);
    }
}