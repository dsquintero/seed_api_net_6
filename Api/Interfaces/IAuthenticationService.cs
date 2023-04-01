
using Api.DTOs;

namespace Api.Interfaces
{
    public interface IAuthenticationService
    {
        Task<AuthenticatedResponse> Login(LoginRequest login);
        Task<AuthenticatedResponse> Refresh(RefreshRequest refreshRequest);
        Task Logout(string UserCode);
    }
}
