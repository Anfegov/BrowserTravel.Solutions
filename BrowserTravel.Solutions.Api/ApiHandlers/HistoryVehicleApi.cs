using BrowserTravel.Solutions.Api.Filters;
using BrowserTravel.Solutions.Application.HistoryVehicle.Commands;
using BrowserTravel.Solutions.Application.HistoryVehicle.Queries;
using BrowserTravel.Solutions.Domain.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace BrowserTravel.Solutions.Api.ApiHandlers;

public static class HistoryVehicleApi
{
    public static RouteGroupBuilder MapHistoryVehicle(this IEndpointRouteBuilder routeHandler)
    {
        /*routeHandler.MapGet("/getById", async (IMediator mediator, Guid id) =>
        {
            return Results.Ok(await mediator.Send(new HistoryVehicleByIdQuery(id)));
        })
        .Produces(StatusCodes.Status200OK, typeof(HistoryVehicleDto))
        .WithMetadata(new AuthorizeAttribute { Roles = "invitado, admin" });

        routeHandler.MapGet("/getAll", async (IMediator mediator) =>
        {
            return Results.Ok(await mediator.Send(new HistoryVehicleGetAllQuery()));
        })
        .Produces(StatusCodes.Status200OK, typeof(List<HistoryVehicleDto>))
        .WithMetadata(new AuthorizeAttribute { Roles = "invitado, admin" });
        */
        routeHandler.MapPost("/linkVehicleToOrigin", async (IMediator mediator, [Validate] LinkVehicleToOriginCommand historyVehicleCommand) =>
        {
            var historyVehicle = await mediator.Send(historyVehicleCommand);

            return Results.Created(new Uri($"/api/historyVehicle/", UriKind.Relative), historyVehicle);
        })
        .Produces(statusCode: StatusCodes.Status201Created)
        /*.WithMetadata(new AuthorizeAttribute { Roles = "invitado, admin" })*/;

        routeHandler.MapGet("/getAvailabilityByOrigin", async (IMediator mediator, Guid origin) =>
        {
            return Results.Ok(await mediator.Send(new GetAvailabilityByOriginQuery(origin)));
        })
        .Produces(StatusCodes.Status200OK, typeof(List<VehicleDto>))
        /*.WithMetadata(new AuthorizeAttribute { Roles = "invitado, admin" })*/;

        routeHandler.MapPost("/registerServiceVehicle", async (IMediator mediator, [Validate] RegisterServiceVehicleCommand registerServiceVehicleCommand) =>
        {
            var historyVehicle = await mediator.Send(registerServiceVehicleCommand);

            return Results.Created(new Uri($"/api/historyVehicle/", UriKind.Relative), historyVehicle);
        })
       .Produces(statusCode: StatusCodes.Status201Created)
       /*.WithMetadata(new AuthorizeAttribute { Roles = "invitado, admin" })*/;

        routeHandler.MapGet("/getActiveServiceByVehicleId", async (IMediator mediator, Guid vehicleId) =>
        {
            return Results.Ok(await mediator.Send(new GetActiveServiceByVehicleIdQuery(vehicleId)));
        })
        .Produces(StatusCodes.Status200OK, typeof(HistoryVehicleDto))
        /*.WithMetadata(new AuthorizeAttribute { Roles = "invitado, admin" })*/;

        routeHandler.MapPut("/ConfirmEndOfServiceById", async (IMediator mediator, Guid historyVehicleId) =>
        {
            return Results.Ok(await mediator.Send(new ConfirmEndOfServiceByIdCommand(historyVehicleId)));
        })
        .Produces(StatusCodes.Status200OK, typeof(HistoryVehicleDto))
        /*.WithMetadata(new AuthorizeAttribute { Roles = "invitado, admin" })*/;

        return (RouteGroupBuilder)routeHandler; 



    }
}
