using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.SuppliesAndMaterialsIssuance.Create.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.SuppliesAndMaterialsIssuance;

public static class CreateSuppliesAndMaterialsIssuanceReportEndpoint
{
    internal static RouteHandlerBuilder MapCreateSuppliesAndMaterialsIssuanceReportEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateSuppliesAndMaterialsIssuanceReportCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request);
                return Results.Created($"/supplies-materials-issuance/{response.Id}", response);
            })
            .WithName(nameof(CreateSuppliesAndMaterialsIssuanceReportEndpoint))
            .WithSummary("creates a supplies and materials issuance report (SMIR)")
            .WithDescription("creates a supplies and materials issuance report (SMIR) to track the movement of inventory from the facility")
            .Produces<CreateSuppliesAndMaterialsIssuanceReportResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .RequirePermission("Permissions.SuppliesAndMaterialsIssuance.Create")
            .MapToApiVersion(1);
    }
}

