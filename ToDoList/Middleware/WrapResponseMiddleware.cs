using System.Text.Json;

namespace ToDoList.Middleware;

public class WrapResponseMiddleware
{
    private readonly RequestDelegate _next;

    public WrapResponseMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        // Save original body stream of the response
        var originalBodyStream = context.Response.Body;

        using var memoryStream = new MemoryStream();
        context.Response.Body = memoryStream;

        // Execute the middleware
        await _next(context);

        // Return to the start of the stream to read the response
        memoryStream.Seek(0, SeekOrigin.Begin);
        var bodyText = await new StreamReader(memoryStream).ReadToEndAsync();

        object result;

        // If the response has content it deserialize it
        if (!string.IsNullOrWhiteSpace(bodyText) &&
            context.Response.ContentType?.Contains("application/json") == true)
        {
            var originalData = JsonSerializer.Deserialize<object>(bodyText, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            result = new
            {
                status = context.Response.StatusCode,
                data = originalData
            };
        }
        else
        {
            // Is not JSON or is empty (for example, 204 No Content)
            result = new
            {
                status = context.Response.StatusCode
            };
        }

        // Rewrites the response in the original body
        context.Response.Body = originalBodyStream;
        context.Response.ContentType = "application/json";
        var json = JsonSerializer.Serialize(result);
        await context.Response.WriteAsync(json);
    }
}