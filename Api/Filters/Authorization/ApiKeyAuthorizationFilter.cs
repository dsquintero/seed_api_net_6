using Api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace Api.Filters.Authorization
{
    public class ApiKeyAuthorizationFilter : IAuthorizationFilter
    {
        private const string HeaderName = "x-api-key";
        private readonly IApiKeyValidator apiKeyValidator;

        public ApiKeyAuthorizationFilter(IApiKeyValidator apiKeyValidator)
        {
            this.apiKeyValidator = apiKeyValidator;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            string apiKey = context.HttpContext.Request.Headers[HeaderName];


            if (string.IsNullOrEmpty(apiKey) || !apiKeyValidator.IsValid(apiKey))
            {
                int statusCode = (int)HttpStatusCode.Unauthorized;
                string message = "No está autorizado para acceder a este recurso.";
                //context.Result = new UnauthorizedResult();
                context.Result = new ObjectResult(new
                {
                    ErrorCode = statusCode,
                    Message = message
                })
                {
                    StatusCode = statusCode
                };
            }
        }
    }
}
