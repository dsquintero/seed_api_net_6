using Api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace Api.Filters.Authorization
{
    public class JwtAuthorizationFilter : IAuthorizationFilter
    {
        private const string HeaderName = "Authorization";

        private readonly IJwtValidator jwtValidator;

        public JwtAuthorizationFilter(IJwtValidator jwtValidator)
        {
            this.jwtValidator = jwtValidator;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            string Jwt = context.HttpContext.Request.Headers[HeaderName];
            string schema = "Bearer";
            if (string.IsNullOrEmpty(Jwt)
                || !Jwt.Contains(schema)
                || !jwtValidator.AccessTokenIsValid(Jwt.Substring(schema.Length).Trim())
                )
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
