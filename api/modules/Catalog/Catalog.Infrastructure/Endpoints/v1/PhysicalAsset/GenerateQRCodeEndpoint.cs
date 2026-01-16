using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.PhysicalAssets.GenerateQRCode.v1;
using Asp.Versioning;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1.PhysicalAsset;

public static class GenerateQRCodeEndpoint
{
    public static void MapGenerateQRCodeEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/{id:guid}/generate-qrcode", async (Guid id, GenerateQRCodeRequest request, ISender mediator) =>
        {
            var command = new GenerateQRCodeCommand(id, request.QRCodeData, request.PropertyNumber);
            var response = await mediator.Send(command);
            return Results.Ok(response);
        })
        .WithName(nameof(GenerateQRCodeEndpoint))
        .WithSummary("Generate QR code for physical asset")
        .WithDescription("Generates and assigns a QR code to a physical asset for tracking purposes")
        .Produces<GenerateQRCodeResponse>()
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequirePermission("Permissions.PhysicalAssets.Edit")
        .MapToApiVersion(new ApiVersion(1, 0));
    }

    private sealed record GenerateQRCodeRequest(string QRCodeData, string? PropertyNumber = null);
}
