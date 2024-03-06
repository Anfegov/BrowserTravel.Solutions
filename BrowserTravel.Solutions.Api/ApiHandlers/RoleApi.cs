using MediatR;
using Microsoft.AspNetCore.Authorization;
using BrowserTravel.Solutions.Api.Filters;
using BrowserTravel.Solutions.Application.Role.Commands;

namespace BrowserTravel.Solutions.Api.ApiHandlers;

public static class RoleApi
{
    // Método de extensión para mapear rutas relacionadas con roles en la API
    public static RouteGroupBuilder MapRole(this IEndpointRouteBuilder routeHandler)
    {
        // Mapear la ruta POST para asignar roles a un usuario
        routeHandler.MapPost("/roleAssignsToUser", async (IMediator mediator, [Validate] RoleAssignsToUserCommand roleCommand) =>
        {
            // Enviar el comando RoleAssignsToUserCommand al mediador para asignar roles a un usuario
            var role = await mediator.Send(roleCommand);

            // Devolver una respuesta 201 Created con la ubicación de la nueva entidad (resultado)
            return Results.Created(new Uri($"/api/role/", UriKind.Relative), role);
        })
        // Especificar que la operación produce un código de estado 201 Created
        .Produces(statusCode: StatusCodes.Status201Created)
        // Agregar el atributo de autorización para permitir solo el rol "admin"
        .WithMetadata(new AuthorizeAttribute { Roles = "admin" });

        // Mapear la ruta POST para crear rol
        routeHandler.MapPost("/roleRegister", async (IMediator mediator, [Validate] RoleRegisterCommand roleCommand) =>
        {
            // Enviar el comando RoleAssignsToUserCommand al mediador para asignar roles a un usuario
            var role = await mediator.Send(roleCommand);

            // Devolver una respuesta 201 Created con la ubicación de la nueva entidad (resultado)
            return Results.Created(new Uri($"/api/role/", UriKind.Relative), role);
        })
        // Especificar que la operación produce un código de estado 201 Created
        .Produces(statusCode: StatusCodes.Status201Created)
        // Agregar el atributo de autorización para permitir solo el rol "admin"
        .WithMetadata(new AuthorizeAttribute { Roles = "admin" });

        // Mapear la ruta POST para actualizar rol
        routeHandler.MapPost("/roleUpdateCommand", async (IMediator mediator, [Validate] RoleUpdateCommand roleCommand) =>
        {
            // Enviar el comando RoleAssignsToUserCommand al mediador para asignar roles a un usuario
            var role = await mediator.Send(roleCommand);

            // Devolver una respuesta 201 Created con la ubicación de la nueva entidad (resultado)
            return Results.Created(new Uri($"/api/role/", UriKind.Relative), role);
        })
        // Especificar que la operación produce un código de estado 201 Created
        .Produces(statusCode: StatusCodes.Status201Created)
        // Agregar el atributo de autorización para permitir solo el rol "admin"
        .WithMetadata(new AuthorizeAttribute { Roles = "admin" });

        // Devolver un objeto RouteGroupBuilder para permitir la configuración adicional
        return (RouteGroupBuilder)routeHandler;
    }
}
