using System.Linq.Expressions;
using GNS.Contracts.Requests;
using GNS.Contracts.Responses;
using GNS.Data.Entities;

namespace GNS.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserEntity?> FindUserAsync(
            Expression<Func<UserEntity, bool>> predicate,
            CancellationToken token = default
            );
        Task RegisterAsync(RegisterUserRequest request, CancellationToken token = default);
        Task<LoginUserResponse> LoginAsync(LoginUserRequest request, CancellationToken token = default);
        Task DeleteUserAsync(CancellationToken token = default);
    }
}