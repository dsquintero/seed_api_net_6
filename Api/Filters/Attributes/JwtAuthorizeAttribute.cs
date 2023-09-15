using Api.Filters.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Filters.Attributes
{
    public class JwtAuthorizeAttribute : ServiceFilterAttribute
    {
        public JwtAuthorizeAttribute()
            : base(typeof(JwtAuthorizationFilter))
        {
        }
    }
}
