using MedievalGame.Api.Responses;
using MedievalGame.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace MedievalGame.Api.Middlewares
{
    public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            var traceId = context.TraceIdentifier;

            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unhandled exception occurred. TraceId: {TraceId}", traceId);
                await HandleExceptionAsync(context, ex, traceId);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception, string traceId)
        {
            var (statusCode, responseBody) = exception switch
            {
                ValidationsException valEx => (
                    valEx.StatusCode,
                    ApiResponse<object>.ErrorResponse(valEx.Message, valEx.StatusCode, traceId)
                        .WithErrors(valEx.Errors)
                ),

                NotFoundException notFoundEx => (
                    StatusCodes.Status404NotFound,
                    ApiResponse<object>.ErrorResponse(notFoundEx.Message, StatusCodes.Status404NotFound, traceId)
                ),

                DomainException domainEx => (
                    domainEx.StatusCode,
                    ApiResponse<object>.ErrorResponse(domainEx.Message, domainEx.StatusCode, traceId)
                ),

                DbUpdateException => (
                    StatusCodes.Status500InternalServerError,
                    ApiResponse<object>.ErrorResponse("A database error occurred.", StatusCodes.Status500InternalServerError, traceId)
                ),

                TaskCanceledException => (
                    StatusCodes.Status408RequestTimeout,
                    ApiResponse<object>.ErrorResponse("The request timed out.", StatusCodes.Status408RequestTimeout, traceId)
                ),

                _ => (
                    StatusCodes.Status500InternalServerError,
                    ApiResponse<object>.ErrorResponse("An unexpected error occurred.", StatusCodes.Status500InternalServerError, traceId)
                )
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            await context.Response.WriteAsJsonAsync(responseBody);
        }
    }
}
