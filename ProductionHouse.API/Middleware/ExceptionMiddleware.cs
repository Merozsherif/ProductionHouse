using System.Net;
using System.Text.Json;
using ProductionHouse.Core.Exceptions;
using ProductionHouse.Core.Responses;

namespace ProductionHouse.API.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        HttpStatusCode statusCode;

        switch (exception)
        {
            case NotFoundException:
                statusCode = HttpStatusCode.NotFound;
                break;

            case BadRequestException:
                statusCode = HttpStatusCode.BadRequest;
                break;

            default:
                statusCode = HttpStatusCode.InternalServerError;
                exception = exception.InnerException ?? exception;
                break;
        }

        context.Response.StatusCode = (int)statusCode;

        var response = new ApiResponse<object>
        {
            Success = false,
            Message = exception.InnerException?.Message ?? exception.Message,
            Errors = null,
            Data = null
        };

        var json = JsonSerializer.Serialize(response);

        await context.Response.WriteAsync(json);
    }
}