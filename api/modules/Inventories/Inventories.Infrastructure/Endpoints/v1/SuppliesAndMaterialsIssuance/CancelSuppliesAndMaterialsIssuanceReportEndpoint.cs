using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.SuppliesAndMaterialsIssuance.Cancel.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.SuppliesAndMaterialsIssuance;

public static class CancelSuppliesAndMaterialsIssuanceReportEndpoint
{
    internal static RouteHandlerBuilder MapCancelSuppliesAndMaterialsIssuanceReportEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id}/cancel", async (Guid id, ISender mediator) =>
            {
                var command = new CancelSuppliesAndMaterialsIssuanceReportCommand(id);
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(CancelSuppliesAndMaterialsIssuanceReportEndpoint))
            .WithSummary("Cancel supplies and materials issuance report (SMIR)")
            .WithDescription("Cancels a SMIR and reverses semi-expendable inventory changes with transaction logs")
            .Produces<CancelSuppliesAndMaterialsIssuanceReportResponse>()
            .Produces(404)
            .RequirePermission("Permissions.Inventories.Cancel")
            .MapToApiVersion(1);
    }
}
