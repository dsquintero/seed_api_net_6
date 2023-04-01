using Api.Interfaces;
using Api.Repositories;
using Api.Services;

namespace Api.Utils
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection addServices(this IServiceCollection services)
        {
            services.AddSingleton<JwtService>();
            services.AddSingleton<IAuthenticationService, AuthenticationService>();
            services.AddSingleton<IAuthenticationRepository, AuthenticationRepository>();
            services.AddSingleton<IPasswordHasherService, PasswordHasherService>();
            services.AddSingleton<IRefreshTokenRepository, RefreshTokenRepository>();

            return services;
        }
    }
}
