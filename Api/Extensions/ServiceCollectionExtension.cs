using Api.Filters.Authorization;
using Api.Interfaces;
using Api.Repositories;
using Api.Services;
using Api.Validators;

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

            services.AddSingleton<JwtAuthorizationFilter>();
            services.AddSingleton<IJwtValidator, JwtValidator>();

            services.AddSingleton<ApiKeyAuthorizationFilter>();
            services.AddSingleton<IApiKeyValidator, ApiKeyValidator>();

            return services;
        }
    }
}
