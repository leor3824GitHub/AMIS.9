using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.SuppliesAndMaterialsIssuance.Update.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Authorization;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.SuppliesAndMaterialsIssuance;

public static class UpdateSuppliesAndMaterialsIssuanceReportEndpoint
{
    internal static RouteHandlerBuilder MapUpdateSuppliesAndMaterialsIssuanceReportEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (Guid id, UpdateSuppliesAndMaterialsIssuanceReportCommand request, ISender mediator) =>
            {
                var command = request with { Id = id };
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdateSuppliesAndMaterialsIssuanceReportEndpoint))
            .WithSummary("updates a supplies and materials issuance report (SMIR)")
            .WithDescription("updates a supplies and materials issuance report (SMIR) - only Accounting personnel can update")
            .Produces<UpdateSuppliesAndMaterialsIssuanceReportResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status404NotFound)
            .RequirePermission($"{FshResources.SuppliesAndMaterialsIssuance}.{FshActions.Update}")
            .MapToApiVersion(1);
    }
}
