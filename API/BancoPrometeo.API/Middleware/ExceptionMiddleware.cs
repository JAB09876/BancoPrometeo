using System.Text.Json;
using Microsoft.Data.SqlClient;

namespace BancoPrometeo.API.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, message) = exception switch
        {
            SqlException sqlEx => MapSqlException(sqlEx),
            UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, exception.Message),
            KeyNotFoundException => (StatusCodes.Status404NotFound, exception.Message),
            InvalidOperationException => (StatusCodes.Status400BadRequest, exception.Message),
            _ => (StatusCodes.Status500InternalServerError, "Ha ocurrido un error interno. Intente de nuevo.")
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        var response = new { error = message, statusCode };
        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }

    private static (int statusCode, string message) MapSqlException(SqlException ex)
    {
        return ex.Number switch
        {
            >= 50001 and <= 50009 => (StatusCodes.Status404NotFound, ex.Message),
            >= 50010 and <= 50019 => (StatusCodes.Status400BadRequest, ex.Message),
            >= 50020 and <= 50029 => (StatusCodes.Status422UnprocessableEntity, ex.Message),
            >= 50030 and <= 50039 => (StatusCodes.Status422UnprocessableEntity, ex.Message),
            >= 50040 and <= 50049 => (StatusCodes.Status400BadRequest, ex.Message),
            >= 50050 and <= 50059 => (StatusCodes.Status400BadRequest, ex.Message),
            >= 50060 and <= 50069 => (StatusCodes.Status400BadRequest, ex.Message),
            >= 50070 and <= 50079 => (StatusCodes.Status400BadRequest, ex.Message),
            >= 50080 and <= 50089 => (StatusCodes.Status400BadRequest, ex.Message),
            >= 50090 and <= 50099 => (StatusCodes.Status409Conflict, ex.Message),
            _ => (StatusCodes.Status500InternalServerError, "Error de base de datos.")
        };
    }
}
