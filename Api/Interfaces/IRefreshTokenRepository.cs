
using Api.Models;

namespace Api.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken> GetByJWT(string JWT);
        Task<int> Create(RefreshToken refreshToken);
        Task<int> Delete(string UserCode);
    }
}
