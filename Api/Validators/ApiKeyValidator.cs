using Api.Interfaces;
using Api.Models;

namespace Api.Validators
{
    public class ApiKeyValidator : IApiKeyValidator
    {
        private readonly EnvironmentConfig env;
        public ApiKeyValidator(EnvironmentConfig env)
        {
            this.env = env;
        }
        public bool IsValid(string apiKey)
        {
            return apiKey.Equals(env.X_Api_Key, StringComparison.OrdinalIgnoreCase);
        }
    }
}
