using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.SuppliesAndMaterialsReceiving.Update.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Authorization;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.SuppliesAndMaterialsReceiving;

public static class UpdateSuppliesAndMaterialsReceivingReportEndpoint
{
    internal static RouteHandlerBuilder MapUpdateSuppliesAndMaterialsReceivingReportEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (Guid id, UpdateSuppliesAndMaterialsReceivingReportCommand request, ISender mediator) =>
            {
                var command = request with { Id = id };
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdateSuppliesAndMaterialsReceivingReportEndpoint))
            .WithSummary("updates a supplies and materials receiving report (SMRR)")
            .WithDescription("updates a supplies and materials receiving report (SMRR) - only Accounting personnel can update")
            .Produces<UpdateSuppliesAndMaterialsReceivingReportResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status404NotFound)
            .RequirePermission($"{FshResources.SuppliesAndMaterialsReceiving}.{FshActions.Update}")
            .MapToApiVersion(1);
    }
}
