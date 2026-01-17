using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.SuppliesAndMaterialsReceiving.Create.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.SuppliesAndMaterialsReceiving;

public static class CreateSuppliesAndMaterialsReceivingReportEndpoint
{
    internal static RouteHandlerBuilder MapCreateSuppliesAndMaterialsReceivingReportEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateSuppliesAndMaterialsReceivingReportCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request);
                return Results.Created($"/supplies-materials-receiving/{response.Id}", response);
            })
            .WithName(nameof(CreateSuppliesAndMaterialsReceivingReportEndpoint))
            .WithSummary("creates a supplies and materials receiving report (SMRR)")
            .WithDescription("creates a supplies and materials receiving report (SMRR) to document the arrival and intake of goods into inventory")
            .Produces<CreateSuppliesAndMaterialsReceivingReportResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .RequirePermission("Permissions.SuppliesAndMaterialsReceiving.Create")
            .MapToApiVersion(1);
    }
}

