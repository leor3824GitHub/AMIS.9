using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.SuppliesAndMaterialsReceiving.Get.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.SuppliesAndMaterialsReceiving;

public static class GetSuppliesAndMaterialsReceivingReportEndpoint
{
    internal static RouteHandlerBuilder MapGetSuppliesAndMaterialsReceivingReportEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (Guid id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetSuppliesAndMaterialsReceivingReportByIdQuery(id));
                return Results.Ok(response);
            })
            .WithName(nameof(GetSuppliesAndMaterialsReceivingReportEndpoint))
            .WithSummary("gets supplies and materials receiving report by id")
            .WithDescription("retrieves a specific supplies and materials receiving report (SMRR) by its ID")
            .Produces<GetSuppliesAndMaterialsReceivingReportByIdResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status401Unauthorized)
            .RequirePermission("Permissions.SuppliesAndMaterialsReceiving.View")
            .MapToApiVersion(1);
    }
}

