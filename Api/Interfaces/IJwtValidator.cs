namespace Api.Interfaces
{
    public interface IJwtValidator
    {
        bool AccessTokenIsValid(string accessToken);
        bool RefreshTokenIsValid(string refreshToken);
    }
}