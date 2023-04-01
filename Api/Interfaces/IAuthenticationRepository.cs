using Api.Models;

namespace Api.Interfaces
{
    public interface IAuthenticationRepository
    {
        Task<User> GetByUserCode(string UserCode);
    }
}
