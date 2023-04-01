using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using System.Text;

namespace Api.Filters
{
    public class GlobalExceptionFilter2 : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            int statusCode;
            Exception ex = context.Exception;

            switch (true)
            {
                case bool _ when ex is UnauthorizedAccessException:
                    statusCode = (int)HttpStatusCode.Unauthorized;
                    break;


                case bool _ when ex is InvalidOperationException:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    break;


                default:
                    statusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            context.Result =
                new ObjectResult
                (
                    new
                    {
                        ErrorCode = statusCode,
                        Message = ExceptionMessage(ex)
                        //,Trace = ex.StackTrace
                    }
                )
                { StatusCode = statusCode };
        }
        private string ExceptionMessage(Exception ex)
        {
            var sb = new StringBuilder();
            var innerEx = ex.InnerException;
            while (innerEx != null)
            {
                sb.Append(innerEx.Message);
                innerEx = innerEx.InnerException;
            }

            return sb.Length > 0 ? sb.ToString() : ex.Message;
        }
        private string ExceptionMessageStackTrace(Exception ex)
        {
            var sb = new StringBuilder();
            var innerEx = ex.InnerException;
            while (innerEx != null)
            {
                sb.AppendLine(innerEx.Message);
                innerEx = innerEx.InnerException;
            }
            sb.AppendLine(ex.Message);
            sb.AppendLine(ex.StackTrace);
            return sb.ToString();
        }
        private string ExceptionStackTrace(Exception ex)
        {
            var sb = new StringBuilder();
            var innerEx = ex.InnerException;
            while (innerEx != null)
            {
                sb.AppendLine(innerEx.StackTrace);
                innerEx = innerEx.InnerException;
            }
            sb.AppendLine(ex.StackTrace);
            return sb.ToString();
        }
    }
}
