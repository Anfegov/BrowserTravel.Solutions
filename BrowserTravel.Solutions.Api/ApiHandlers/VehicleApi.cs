using MediatR;
using Microsoft.AspNetCore.Authorization;
using BrowserTravel.Solutions.Api.Filters;
using BrowserTravel.Solutions.Application.Vehicle.Commands;
using BrowserTravel.Solutions.Application.Vehicle.Queries;
using BrowserTravel.Solutions.Domain.Dtos;

namespace BrowserTravel.Solutions.Api.ApiHandlers;

public static class VehicleApi
{
    public static RouteGroupBuilder MapVehicle(this IEndpointRouteBuilder routeHandler)
    {
        routeHandler.MapGet("/getById", async (IMediator mediator, Guid id) =>
        {
            return Results.Ok(await mediator.Send(new VehicleByIdQuery(id)));
        })
        .Produces(StatusCodes.Status200OK, typeof(VehicleDto))
        /*.WithMetadata(new AuthorizeAttribute { Roles = "invitado, admin" })*/;

        routeHandler.MapGet("/getAll", async (IMediator mediator) =>
        {
            return Results.Ok(await mediator.Send(new VehicleGetAllQuery()));
        })
        .Produces(StatusCodes.Status200OK, typeof(List<VehicleDto>))
        /*.WithMetadata(new AuthorizeAttribute { Roles = "invitado, admin" })*/;

        routeHandler.MapPost("/", async (IMediator mediator, [Validate] VehicleRegisterCommand vehicleCommand) =>
        {
            var vehicle = await mediator.Send(vehicleCommand);

            return Results.Created(new Uri($"/api/vehicle/{vehicle.Id}", UriKind.Relative), vehicle);
        })
        .Produces(statusCode: StatusCodes.Status201Created)
        /*.WithMetadata(new AuthorizeAttribute { Roles = "invitado, admin" })*/;

        return (RouteGroupBuilder)routeHandler;
    }
}
