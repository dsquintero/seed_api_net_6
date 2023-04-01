using Api.DTOs;
using Api.Interfaces;
using Api.Models;
using AutoMapper;

namespace Api.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IAuthenticationRepository authenticationRepository;
        private readonly IPasswordHasherService passwordHasher;
        private readonly IMapper mapper;
        private readonly JwtService jwt;
        private readonly IRefreshTokenRepository refreshTokenRepository;

        public AuthenticationService(
            IAuthenticationRepository authenticationRepository,
            IPasswordHasherService passwordHasher,
            IMapper mapper,
            JwtService jwt,
            IRefreshTokenRepository refreshTokenRepository)
        {
            this.authenticationRepository = authenticationRepository;
            this.passwordHasher = passwordHasher;
            this.mapper = mapper;
            this.jwt = jwt;
            this.refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<AuthenticatedResponse> Login(LoginRequest login)
        {
            // Validadr Modelo !ModelState.IsValid

            User user = await authenticationRepository.GetByUserCode(login.UserCode);
            if (user == null)
            {
                throw new InvalidOperationException("El usuario no se encuentra registrado");
            }

            bool isCorrectPassword = passwordHasher.VerifyPassword(login.Password, user.Password);
            if (!isCorrectPassword)
            {
                throw new InvalidOperationException("Lo contraseña no es correcta");
            }

            LoginResponse loginResponse = mapper.Map<LoginResponse>(user);

            return await JWTAuthenticate(loginResponse);
        }
        public async Task<AuthenticatedResponse> Refresh(RefreshRequest refreshRequest)
        {
            bool isValidRefreshToken = jwt.RefreshTokenValidate(refreshRequest.RefreshToken);
            if (!isValidRefreshToken)
            {
                throw new InvalidOperationException("Invalid refresh token.");
            }

            RefreshToken refreshTokenDto = await refreshTokenRepository.GetByJWT(refreshRequest.RefreshToken);
            if (refreshTokenDto == null)
            {
                throw new InvalidOperationException("Invalid refresh token.");
            }

            await refreshTokenRepository.Delete(refreshTokenDto.UserCode);

            LoginResponse loginResponse = await GetByUserCode(refreshTokenDto.UserCode);

            return await JWTAuthenticate(loginResponse);
        }
        public async Task Logout(string UserCode)
        {
            await refreshTokenRepository.Delete(UserCode);
        }

        private async Task<AuthenticatedResponse> JWTAuthenticate(LoginResponse user)
        {
            string accessToken = jwt.GenerateAccessToken(user);
            string refreshToken = jwt.GenerateRefreshToken();

            RefreshToken refreshTokenDto = new RefreshToken()
            {
                JWT = refreshToken,
                UserCode = user.UserCode
            };

            await refreshTokenRepository.Create(refreshTokenDto);

            return new AuthenticatedResponse()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }
        private async Task<LoginResponse> GetByUserCode(string UserCode)
        {
            User user = await authenticationRepository.GetByUserCode(UserCode);
            if (user == null)
            {
                throw new InvalidOperationException("El usuario no se encuentra registrado");
            }
            return mapper.Map<LoginResponse>(user);
        }
    }
}
