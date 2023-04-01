using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace Api.Filters
{
    public class ValidationAttributeFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                IEnumerable<string> errorMessages = context.ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
                int statusCode = (int)HttpStatusCode.UnprocessableEntity;
                context.Result =
                new ObjectResult
                (
                    new
                    {
                        ErrorCode = statusCode,
                        ErrorMessages = errorMessages
                    }
                )
                { StatusCode = statusCode };
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }
    }
}
