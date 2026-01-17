using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.SuppliesAndMaterialsIssuance.Get.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.SuppliesAndMaterialsIssuance;

public static class GetSuppliesAndMaterialsIssuanceReportEndpoint
{
    internal static RouteHandlerBuilder MapGetSuppliesAndMaterialsIssuanceReportEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (Guid id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetSuppliesAndMaterialsIssuanceReportByIdQuery(id));
                return Results.Ok(response);
            })
            .WithName(nameof(GetSuppliesAndMaterialsIssuanceReportEndpoint))
            .WithSummary("gets supplies and materials issuance report by id")
            .WithDescription("retrieves a specific supplies and materials issuance report (SMIR) by its ID")
            .Produces<GetSuppliesAndMaterialsIssuanceReportByIdResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status401Unauthorized)
            .RequirePermission("Permissions.SuppliesAndMaterialsIssuance.View")
            .MapToApiVersion(1);
    }
}

