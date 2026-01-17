using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.PhysicalAssets.Export.v1;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.PhysicalAsset;

public static class ExportPhysicalAssetsEndpoint
{
    public static void MapExportPhysicalAssetsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/export", async ([FromBody] ExportPhysicalAssetsRequest request, ISender mediator) =>
        {
            var command = new ExportPhysicalAssetsCommand(
                request.Classification,
                request.Location,
                request.Condition,
                request.IsDisposed);
            
            var response = await mediator.Send(command);
            return Results.File(response.FileContent, response.ContentType, response.FileName);
        })
        .WithName(nameof(ExportPhysicalAssetsEndpoint))
        .WithSummary("Export physical assets to CSV")
        .WithDescription("Exports filtered physical assets to a CSV file for reporting purposes")
        .Produces<FileResult>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .RequirePermission("Permissions.PhysicalAssets.View")
        .MapToApiVersion(new ApiVersion(1, 0));
    }

    private sealed record ExportPhysicalAssetsRequest(
        string? Classification = null,
        string? Location = null,
        string? Condition = null,
        bool? IsDisposed = null);
}

