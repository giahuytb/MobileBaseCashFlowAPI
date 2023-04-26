
using MobileBasedCashFlowAPI.Exceptions;
using System.Net;
using System.Text.Json;

using KeyNotFoundException = MobileBasedCashFlowAPI.Exceptions.KeyNotFoundException;
using NotImplementedException = MobileBasedCashFlowAPI.Exceptions.NotImplementedException;
using UnauthorizedAccessException = MobileBasedCashFlowAPI.Exceptions.UnauthorizedAccessException;

namespace MobileBasedCashFlowAPI.Extensions
{
    public class GlobalExceptionHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

        public GlobalExceptionHandlingMiddleware(
            ILogger<GlobalExceptionHandlingMiddleware> logger) => _logger = logger;


        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                await HandleExceptionAsync(context, ex);

                //context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                //ProblemDetails problem = new()
                //{
                //    Status = (int)HttpStatusCode.InternalServerError,
                //    Type = "Server Error",
                //    Title = "Server Error",
                //    Detail = "An Server Error Has Occurred"
                //};

                //string json = JsonSerializer.Serialize(problem);

                //context.Response.ContentType = "application/json";

                //await context.Response.WriteAsync(json);

            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            HttpStatusCode status;
            var stackTrace = String.Empty;
            string message;
            var exceptionType = exception.GetType();
            if (exceptionType == typeof(BadRequestException))
            {
                message = exception.Message;
                status = HttpStatusCode.BadRequest;
                stackTrace = exception.StackTrace;
            }
            else if (exceptionType == typeof(NotFoundException))
            {
                message = exception.Message;
                status = HttpStatusCode.NotFound;
                stackTrace = exception.StackTrace;
            }
            else if (exceptionType == typeof(NotImplementedException))
            {
                status = HttpStatusCode.NotImplemented;
                message = exception.Message;
                stackTrace = exception.StackTrace;
            }
            else if (exceptionType == typeof(UnauthorizedAccessException))
            {
                status = HttpStatusCode.Unauthorized;
                message = exception.Message;
                stackTrace = exception.StackTrace;
            }
            else if (exceptionType == typeof(KeyNotFoundException))
            {
                status = HttpStatusCode.Unauthorized;
                message = exception.Message;
                stackTrace = exception.StackTrace;
            }
            else
            {
                status = HttpStatusCode.InternalServerError;
                message = exception.Message;
                stackTrace = exception.StackTrace;
            }
            var exceptionResult = JsonSerializer.Serialize(new
            {
                error = message,
                stackTrace
            });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)status;
            return context.Response.WriteAsync(exceptionResult);
        }
    }
}
