using Serilog.Context;
using System.Text;
using System.Text.Json;

namespace MedievalGame.Api.Middlewares
{
    public class RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
    {
        public async Task Invoke(HttpContext context)
        {
            var request = context.Request;
            var originalBodyStream = context.Response.Body;

            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            await next(context);

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseText = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);

            var responseMessage = TryExtractMessage(responseText);

            using (LogContext.PushProperty("LogType", "EndpointLog"))
            {
                logger.LogInformation(
                    "📥 Request to {Method} {Path} / 📤 Response: {StatusCode} / 📦 Message: {Message}",
                    request.Method,
                    request.Path,
                    context.Response.StatusCode,
                    responseMessage
                );
            }

            await responseBody.CopyToAsync(originalBodyStream);
        }

        private string TryExtractMessage(string responseBody)
        {
            try
            {
                using var doc = JsonDocument.Parse(responseBody);
                var root = doc.RootElement;

                if (root.TryGetProperty("message", out var message))
                {
                    return message.GetString() ?? "No message";
                }

                if (root.TryGetProperty("title", out var title) &&
                    root.TryGetProperty("errors", out var errors))
                {
                    var sb = new StringBuilder();
                    sb.AppendLine(title.GetString() ?? "Validation error");

                    foreach (var error in errors.EnumerateObject())
                    {
                        var field = error.Name;
                        var messages = error.Value.EnumerateArray().Select(m => m.GetString());
                        foreach (var msg in messages)
                        {
                            sb.AppendLine($"- {field}: {msg}");
                        }
                    }

                    return sb.ToString();
                }
            }
            catch
            {
                // Ignoramos errores de parseo
            }

            return "No message or invalid JSON";
        }
    }
}
