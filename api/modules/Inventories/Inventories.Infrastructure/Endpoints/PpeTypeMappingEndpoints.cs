using AMIS.WebApi.Inventories.Application.PPETypeMappings.Create.v1;
using AMIS.WebApi.Inventories.Application.PPETypeMappings.List.v1;
using AMIS.WebApi.Inventories.Application.PPETypeMappings.ToggleStatus.v1;
using AMIS.WebApi.Inventories.Application.PPETypeMappings.Update.v1;
using Asp.Versioning;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using AMIS.Framework.Infrastructure.Auth.Policy;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints;

public static class PPETypeMappingEndpoints
{
    public static RouteGroupBuilder MapPPETypeMappingEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/ppe-type-mappings")
            .WithTags("PPE Type Mappings");

        group.MapGet("/", async (ISender sender) =>
        {
            var result = await sender.Send(new ListPPETypeMappingsQuery());
            return Results.Ok(result);
        })
        .WithName("ListPPETypeMappings")
        .Produces<List<PPETypeMappingResponse>>()
        .RequirePermission("Permissions.PPETypeMappings.View")
        .MapToApiVersion(new ApiVersion(1, 0));

        group.MapPost("/", async (CreatePPETypeMappingCommand command, ISender sender) =>
        {
            var id = await sender.Send(command);
            return Results.Created($"/ppe-type-mappings/{id}", new { id });
        })
        .WithName("CreatePPETypeMapping")
        .Produces<Guid>(StatusCodes.Status201Created)
        .RequirePermission("Permissions.PPETypeMappings.Manage")
        .MapToApiVersion(new ApiVersion(1, 0));

        group.MapPut("/{id:guid}", async (Guid id, UpdatePPETypeMappingCommand command, ISender sender) =>
        {
            if (id != command.Id)
                return Results.BadRequest("ID mismatch");

            await sender.Send(command);
            return Results.NoContent();
        })
        .WithName("UpdatePPETypeMapping")
        .Produces(StatusCodes.Status204NoContent)
        .RequirePermission("Permissions.PPETypeMappings.Manage")
        .MapToApiVersion(new ApiVersion(1, 0));

        group.MapPatch("/{id:guid}/toggle-status", async (Guid id, TogglePPETypeMappingStatusCommand command, ISender sender) =>
        {
            if (id != command.Id)
                return Results.BadRequest("ID mismatch");

            await sender.Send(command);
            return Results.NoContent();
        })
        .WithName("TogglePPETypeMappingStatus")
        .Produces(StatusCodes.Status204NoContent)
        .RequirePermission("Permissions.PPETypeMappings.Manage")
        .MapToApiVersion(new ApiVersion(1, 0));

        return group;
    }
}

