using Azure.Core;
using BrowserTravel.Solutions.Domain.Exceptions;
using System.Net;

namespace BrowserTravel.Solutions.Api.Middleware;

// Middleware para manejar excepciones de la aplicación
public class AppExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<AppExceptionHandlerMiddleware> _logger;

    // Constructor del middleware
    public AppExceptionHandlerMiddleware(RequestDelegate next, ILogger<AppExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    // Método de invocación del middleware
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            // Invocar el siguiente middleware en la cadena de middlewares
            await _next.Invoke(context);
        }
        catch (Exception ex)
        {
            // Capturar y manejar la excepción
            _logger.LogError(ex, ex.Message);

            // Serializar el mensaje de error como JSON
            var result = System.Text.Json.JsonSerializer.Serialize(new
            {
                ErrorMessage = ex.Message
            });

            // Configurar la respuesta HTTP con el tipo de contenido y el código de estado adecuados
            context.Response.ContentType = ContentType.ApplicationJson.ToString();
            context.Response.StatusCode =
                (ex is CoreException) ? (int)HttpStatusCode.BadRequest : (int)HttpStatusCode.InternalServerError;

            // Escribir el resultado JSON en la respuesta HTTP
            await context.Response.WriteAsync(result);
        }
    }
}
