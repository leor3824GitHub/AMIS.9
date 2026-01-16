using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.PhysicalAssets.RecordDepreciation.v1;
using Asp.Versioning;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1.PhysicalAsset;

public static class RecordDepreciationEndpoint
{
    public static void MapRecordDepreciationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/{id:guid}/record-depreciation", async (Guid id, RecordDepreciationRequest request, ISender mediator) =>
        {
            var command = new RecordDepreciationCommand(id, request.Amount, request.DepreciationDate);
            var response = await mediator.Send(command);
            return Results.Ok(response);
        })
        .WithName(nameof(RecordDepreciationEndpoint))
        .WithSummary("Record depreciation for PPE asset")
        .WithDescription("Records depreciation expense for Property, Plant and Equipment assets")
        .Produces<RecordDepreciationResponse>()
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequirePermission("Permissions.PhysicalAssets.Edit")
        .MapToApiVersion(new ApiVersion(1, 0));
    }

    private sealed record RecordDepreciationRequest(decimal Amount, DateTime DepreciationDate);
}
