using GNS.Contracts;
using GNS.Contracts.Requests;
using GNS.Enums;

namespace GNS.Services.Interfaces
{
    public interface IUserService
    {
        Task Register(RegisterUserRequest request);
        Task<string> Login(LoginUserRequest request);
        Task DeleteUser();
    }
}