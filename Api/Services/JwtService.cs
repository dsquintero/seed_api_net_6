using Api.DTOs;
using Api.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Api.Services
{
    public class JwtService
    {
        private readonly EnvironmentConfig env;

        public JwtService(EnvironmentConfig env)
        {
            this.env = env;
        }

        #region RefreshToken
        public string GenerateRefreshToken()
        {
            return GenerateToken(
                env.JWT_RefreshTokenSecret,
                env.JWT_Issuer,
                env.JWT_Audience,
                env.JWT_RefreshTokenExpirationMinutes
                );
        }

        #endregion

        #region AccessToken
        public string GenerateAccessToken(LoginResponse user)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim("UserCode",user.UserCode),
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(ClaimTypes.Email,user.Email),
            };

            return GenerateToken(
                env.JWT_AccessTokenSecret,
                env.JWT_Issuer,
                env.JWT_Audience,
                env.JWT_AccessTokenExpirationMinutes,
                claims
                );

        }

        #endregion

        private string GenerateToken(string secretKey, string issuer, string audience, double expirationMinutes, IEnumerable<Claim> claims = null)
        {
            SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer,
                audience,
                claims,
                DateTime.UtcNow,
                DateTime.UtcNow.AddMinutes(expirationMinutes),
                credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
