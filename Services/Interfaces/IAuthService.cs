using GNS.Contracts.Responses;

namespace GNS.Services.Interfaces
{
    public interface IAuthService
    {
        Task<string> GetNewAcessTokenAsync(Guid userId, CancellationToken token = default);
        Task<VerifyRefreshTokenResponse> VerifyRefreshTokenAsync(
            string tokenValue,
            Guid userId,
            CancellationToken token = default
            );
        Task<bool> VerifyOwnerAccessToCyberClubAsync(
            Guid ownerId,
            Guid cyberClubId,
            CancellationToken token = default
            );
        Task<bool> VerifyOwnerAccessToEmployeeAsync(
            Guid ownerId,
            Guid employeeId,
            CancellationToken token = default
            );
        Task<bool> VerifyManagerAccessToEmployeeAsync(
            Guid managerId,
            Guid employeeId,
            CancellationToken token = default
            );
        Task<bool> VerifyEmployeeAccessToGamingPlaceAsync(
            Guid employeeId,
            Guid gamingPlaceId,
            CancellationToken token = default
            );
        Task<bool> VerifyEmployeeAccessToOrderAsync(
            Guid employeeId,
            Guid orderId,
            CancellationToken token = default
            );
        Task<bool> VerifyOwnerAccessToGamingPlaceAsync(
            Guid ownerId,
            Guid gamingPlaceId,
            CancellationToken token = default
            );
        Task<bool> VerifyOwnerAccessToWorkingHoursAsync(
            Guid ownerId,
            Guid workingHoursId,
            CancellationToken token = default
            );

        Task<bool> VerifyManagerAccessToCyberClubAsync(
            Guid managerId,
            Guid cyberClubId,
            CancellationToken token = default
            );
    }
}