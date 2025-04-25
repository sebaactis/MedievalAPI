namespace MedievalGame.Api.Responses;

public class ApiResponse<T>
{
    public bool Success { get; init; }
    public string? Message { get; init; }
    public T? Data { get; init; }
    public int StatusCode { get; init; }
    public IEnumerable<string>? Errors { get; init; }
    public DateTime Timestamp { get; } = DateTime.UtcNow;
    public string? TraceId { get; set; }

    public static ApiResponse<T> SuccessResponse(T data, string? message = null, int statusCode = 200, string? traceId = null)
        => new() { Success = true, Data = data, Message = message, StatusCode = statusCode, TraceId = traceId };

    public static ApiResponse<T> ErrorResponse(string message, int statusCode, T? data = default, string? traceId = null)
        => new() { Success = false, Data = data, Message = message, StatusCode = statusCode, TraceId = traceId };

    public ApiResponse<T> WithErrors(IEnumerable<string> errors)
    {
        return new ApiResponse<T>
        {
            Success = this.Success,
            Message = this.Message,
            Data = this.Data,
            StatusCode = this.StatusCode,
            Errors = errors,
        };
    }
}