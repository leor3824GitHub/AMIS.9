using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.PropertyCodes.Generate.v1;
using AMIS.WebApi.Inventories.Domain.ValueObjects;
using Asp.Versioning;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.PropertyCode;

public static class GeneratePropertyCodeEndpoint
{
    public static void MapGeneratePropertyCodeEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("/generate", GenerateHandler)
            .WithName(nameof(GeneratePropertyCodeEndpoint))
            .WithSummary("Generate a property code sequence")
            .WithDescription("Generates the next sequence for a property code. Returns Classification, Category, and Sequence. UI formats as: {Year}-NFA-{Office}-{Classification}-{Category}-{Sequence}")
            .Produces<GeneratePropertyCodeResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequirePermission("Permissions.PhysicalAssets.Create")
            .MapToApiVersion(new ApiVersion(1, 0));
    }

    private static async Task<IResult> GenerateHandler(
        GeneratePropertyCodeRequest request,
        ISender mediator,
        CancellationToken cancellationToken = default)
    {
        var command = new GeneratePropertyCodeCommand(
            AcquisitionDate: request.AcquisitionDate ?? DateTime.UtcNow,
            Classification: request.Classification ?? PropertyClassification.SemiExpendable,
            OfficeCode: request.OfficeCode,
            ClassCode: request.ClassCode,
            CategoryCode: request.CategoryCode,
            ItemCode: request.ItemCode,
            SequenceSuffix: request.SequenceSuffix);

        var response = await mediator.Send(command, cancellationToken);
        return Results.Ok(response);
    }

    public sealed record GeneratePropertyCodeRequest(
        DateTime? AcquisitionDate = null,
        PropertyClassification? Classification = null,
        string? OfficeCode = null,
        string? ClassCode = null,
        string? CategoryCode = null,
        string? ItemCode = null,
        string? SequenceSuffix = null);
}
