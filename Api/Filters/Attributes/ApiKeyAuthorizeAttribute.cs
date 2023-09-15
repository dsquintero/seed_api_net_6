using Api.Filters.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Filters.Attributes
{
    public class ApiKeyAuthorizeAttribute : ServiceFilterAttribute
    {
        public ApiKeyAuthorizeAttribute()
            : base(typeof(ApiKeyAuthorizationFilter))
        {
        }
    }
}
