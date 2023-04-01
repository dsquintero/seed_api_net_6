using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace Api.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            int statusCode = (int)HttpStatusCode.InternalServerError;
            string message = "Se ha producido un error inesperado en el servidor.";

            if (context.Exception is UnauthorizedAccessException)
            {
                statusCode = (int)HttpStatusCode.Unauthorized;
                message = "No está autorizado para acceder a este recurso.";
            }
            else if (context.Exception is InvalidOperationException)
            {
                statusCode = (int)HttpStatusCode.BadRequest;
                message = "La solicitud no se pudo procesar debido a una operación no válida.";
            }

            // Log the full error message and stack trace for debugging purposes
            //_logger.LogError(context.Exception, "Error processing request");

            // Return a user-friendly error message in the response
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
