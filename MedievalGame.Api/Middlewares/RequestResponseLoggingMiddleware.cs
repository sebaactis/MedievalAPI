using System.Text;

namespace MedievalGame.Api.Middlewares
{
    public class RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
    {
        public async Task Invoke(HttpContext context)
        {
            context.Request.EnableBuffering();

            var requestBody = string.Empty;

            if (context.Request.ContentLength > 0)
            {
                using var reader = new StreamReader(
                    context.Request.Body,
                    encoding: Encoding.UTF8,
                    detectEncodingFromByteOrderMarks: false,
                    leaveOpen: true
                    );

                requestBody = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0;
            }

            logger.LogInformation("Incoming Request: {method} {url} \nBody: {body}",
            context.Request.Method,
            context.Request.Path,
            requestBody);

            var originalBodyStream = context.Response.Body;
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            await next(context);

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseText = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);

            logger.LogInformation("Response: {statusCode} \nBody: {body}",
                context.Response.StatusCode,
                responseText);

            await responseBody.CopyToAsync(originalBodyStream);
        }
    }
}
