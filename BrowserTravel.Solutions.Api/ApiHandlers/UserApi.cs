using MediatR;
using Microsoft.AspNetCore.Authorization;
using BrowserTravel.Solutions.Api.Filters;
using BrowserTravel.Solutions.Application.User.Commands;
using BrowserTravel.Solutions.Application.User.Queries;

namespace BrowserTravel.Solutions.Api.ApiHandlers;

public static class UserApi
{
    // Método de extensión para mapear rutas relacionadas con usuarios en la API
    public static RouteGroupBuilder MapUser(this IEndpointRouteBuilder routeHandler)
    {
        // Mapear la ruta POST para registrar un nuevo usuario
        routeHandler.MapPost("/", async (IMediator mediator, [Validate] UserRegisterCommand userCommand) =>
        {
            // Enviar el comando UserRegisterCommand al mediador para registrar un nuevo usuario
            var user = await mediator.Send(userCommand);

            // Devolver una respuesta 201 Created con la ubicación de la nueva entidad (usuario)
            return Results.Created(new Uri($"/api/user/", UriKind.Relative), user);
        })
        // Especificar que la operación produce un código de estado 201 Created
        .Produces(statusCode: StatusCodes.Status201Created)
        // Agregar el atributo de autorización para permitir solo el rol "admin"
        .WithMetadata(new AuthorizeAttribute { Roles = "admin" });

        // Mapear la ruta GET para autenticar un usuario por su nombre de usuario y contraseña
        routeHandler.MapGet("/Login", async (IMediator mediator, string userName, string password) =>
        {
            // Enviar el comando UserLoginQuery al mediador para autenticar un usuario
            var user = await mediator.Send(new UserLoginQuery(userName, password));

            // Devolver una respuesta 201 Created con la ubicación de la nueva entidad (usuario)
            return Results.Created(new Uri($"/api/user/", UriKind.Relative), user);
        })
        // Especificar que la operación produce un código de estado 201 Created
        .Produces(statusCode: StatusCodes.Status201Created);

        // Devolver un objeto RouteGroupBuilder para permitir la configuración adicional
        return (RouteGroupBuilder)routeHandler;
    }
}
