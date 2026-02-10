using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.PropertyCodeSequences.List.v1;
using AMIS.WebApi.Inventories.Application.PropertyCodeSequences.GetById.v1;
using AMIS.WebApi.Inventories.Application.PropertyCodeSequences.Create.v1;
using AMIS.WebApi.Inventories.Application.PropertyCodeSequences.Update.v1;
using AMIS.WebApi.Inventories.Application.PropertyCodeSequences.Delete.v1;
using AMIS.WebApi.Inventories.Application.PropertyCodeSequences.AllocateAndIncrement.v1;
using Asp.Versioning;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.PropertyCodeSequence;

public static class PropertyCodeSequenceEndpoints
{
    public static void MapPropertyCodeSequenceListEndpoint(this RouteGroupBuilder group)
    {
        group.MapGet("/", ListHandler)
            .WithName(nameof(PropertyCodeSequenceEndpoints) + "List")
            .WithSummary("List property code sequences")
            .WithDescription("Returns a paginated list of property code sequences with optional search")
            .Produces<ListPropertyCodeSequencesResponse>(StatusCodes.Status200OK)
            .RequirePermission("Permissions.PhysicalAssets.View")
            .MapToApiVersion(new ApiVersion(1, 0));
    }

    public static void MapPropertyCodeSequenceGetEndpoint(this RouteGroupBuilder group)
    {
        group.MapGet("/{id:int}", GetHandler)
            .WithName(nameof(PropertyCodeSequenceEndpoints) + "Get")
            .WithSummary("Get property code sequence by ID")
            .WithDescription("Returns a specific property code sequence")
            .Produces<GetPropertyCodeSequenceResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.PhysicalAssets.View")
            .MapToApiVersion(new ApiVersion(1, 0));
    }

    public static void MapPropertyCodeSequenceCreateEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("/", CreateHandler)
            .WithName(nameof(PropertyCodeSequenceEndpoints) + "Create")
            .WithSummary("Create a new property code sequence")
            .WithDescription("Creates a new property code sequence tracker")
            .Produces<CreatePropertyCodeSequenceResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequirePermission("Permissions.PhysicalAssets.Create")
            .MapToApiVersion(new ApiVersion(1, 0));
    }

    public static void MapPropertyCodeSequenceUpdateEndpoint(this RouteGroupBuilder group)
    {
        group.MapPut("/{id:int}", UpdateHandler)
            .WithName(nameof(PropertyCodeSequenceEndpoints) + "Update")
            .WithSummary("Update property code sequence")
            .WithDescription("Updates an existing property code sequence")
            .Produces<UpdatePropertyCodeSequenceResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequirePermission("Permissions.PropertyCodeSequences.Update")
            .MapToApiVersion(new ApiVersion(1, 0));
    }

    public static void MapPropertyCodeSequenceDeleteEndpoint(this RouteGroupBuilder group)
    {
        group.MapDelete("/{id:int}", DeleteHandler)
            .WithName(nameof(PropertyCodeSequenceEndpoints) + "Delete")
            .WithSummary("Delete property code sequence")
            .WithDescription("Deletes a property code sequence")
            .Produces<DeletePropertyCodeSequenceResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.PhysicalAssets.Delete")
            .MapToApiVersion(new ApiVersion(1, 0));
    }

    public static void MapPropertyCodeSequenceAllocateEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("/allocate", AllocateHandler)
            .WithName(nameof(PropertyCodeSequenceEndpoints) + "Allocate")
            .WithSummary("Allocate next property code sequence")
            .WithDescription("Atomically allocates and returns the next sequence number plus code components for the given classification and item description")
            .Produces<AllocatePropertyCodeSequenceResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequirePermission("Permissions.PhysicalAssets.Create")
            .MapToApiVersion(new ApiVersion(1, 0));
    }

    private static async Task<IResult> ListHandler(
        ISender mediator,
        int pageNumber = 1,
        int pageSize = 10,
        string? searchTerm = null,
        CancellationToken cancellationToken = default)
    {
        var command = new ListPropertyCodeSequencesCommand(pageNumber, pageSize, searchTerm);
        var response = await mediator.Send(command, cancellationToken);
        return Results.Ok(response);
    }

    private static async Task<IResult> GetHandler(
        int id,
        ISender mediator,
        CancellationToken cancellationToken = default)
    {
        var command = new GetPropertyCodeSequenceCommand(id);
        var response = await mediator.Send(command, cancellationToken);
        return Results.Ok(response);
    }

    private static async Task<IResult> CreateHandler(
        CreatePropertyCodeSequenceRequest request,
        ISender mediator,
        CancellationToken cancellationToken = default)
    {
        var command = new CreatePropertyCodeSequenceCommand(request.Classification, request.Category);
        var response = await mediator.Send(command, cancellationToken);
        return Results.CreatedAtRoute(
            nameof(PropertyCodeSequenceEndpoints) + "Get",
            new { id = response.Id },
            response);
    }

    private static async Task<IResult> UpdateHandler(
        int id,
        UpdatePropertyCodeSequenceRequest request,
        ISender mediator,
        CancellationToken cancellationToken = default)
    {
        var command = new UpdatePropertyCodeSequenceCommand(
            id,
            request.Classification,
            request.Category,
            request.LastSequenceValue);
        var response = await mediator.Send(command, cancellationToken);
        return Results.Ok(response);
    }

    private static async Task<IResult> DeleteHandler(
        int id,
        ISender mediator,
        CancellationToken cancellationToken = default)
    {
        var command = new DeletePropertyCodeSequenceCommand(id);
        var response = await mediator.Send(command, cancellationToken);
        return Results.Ok(response);
    }

    private static async Task<IResult> AllocateHandler(
        AllocatePropertyCodeSequenceRequest request,
        ISender mediator,
        CancellationToken cancellationToken = default)
    {
        var command = new AllocatePropertyCodeSequenceCommand(
            request.Classification,
            request.ItemDescription);
        var response = await mediator.Send(command, cancellationToken);
        return Results.Ok(response);
    }

    public sealed record CreatePropertyCodeSequenceRequest(
        string Classification,
        string Category);

    public sealed record UpdatePropertyCodeSequenceRequest(
        string Classification,
        string Category,
        int LastSequenceValue);

    public sealed record AllocatePropertyCodeSequenceRequest(
        string Classification,
        string ItemDescription);

    public sealed record AllocatePropertyCodeSequenceResponse(
        string ClassCode,
        string CategoryCode,
        string ItemCode,
        int NextSequenceNumber);
}
