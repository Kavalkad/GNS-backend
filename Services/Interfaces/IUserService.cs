using GNS.Contracts;
using GNS.Contracts.Requests;
using GNS.Contracts.Responses;
using GNS.Enums;

namespace GNS.Services.Interfaces
{
    public interface IUserService
    {
        Task Register(RegisterUserRequest request);
        Task<LoginUserResponse> Login(LoginUserRequest request);
        Task DeleteUser();
    }
}