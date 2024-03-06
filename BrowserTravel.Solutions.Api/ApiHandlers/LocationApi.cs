using BrowserTravel.Solutions.Api.Filters;
using BrowserTravel.Solutions.Application.Location.Commands;
using BrowserTravel.Solutions.Application.Location.Queries;
using BrowserTravel.Solutions.Domain.Dtos;
using MediatR;

namespace BrowserTravel.Solutions.Api.ApiHandlers;

public static class LocationApi
{
    public static RouteGroupBuilder MapLocation(this IEndpointRouteBuilder routeHandler)
    {
        routeHandler.MapGet("/getById", async (IMediator mediator, Guid id) =>
        {
            return Results.Ok(await mediator.Send(new LocationByIdQuery(id)));
        })
        .Produces(StatusCodes.Status200OK, typeof(LocationDto))
        /*.WithMetadata(new AuthorizeAttribute { Roles = "invitado, admin" })*/;

        routeHandler.MapGet("/getAll", async (IMediator mediator) =>
        {
            return Results.Ok(await mediator.Send(new LocationGetAllQuery()));
        })
        .Produces(StatusCodes.Status200OK, typeof(List<LocationDto>))
        /*.WithMetadata(new AuthorizeAttribute { Roles = "invitado, admin" })*/;

        routeHandler.MapPost("/", async (IMediator mediator, [Validate] LocationRegisterCommand locationCommand) =>
        {
            var location = await mediator.Send(locationCommand);

            return Results.Created(new Uri($"/api/location/{location.Id}", UriKind.Relative), location);
        })
        .Produces(statusCode: StatusCodes.Status201Created)
        /*.WithMetadata(new AuthorizeAttribute { Roles = "invitado, admin" })*/;

        return (RouteGroupBuilder)routeHandler;
    }
}
