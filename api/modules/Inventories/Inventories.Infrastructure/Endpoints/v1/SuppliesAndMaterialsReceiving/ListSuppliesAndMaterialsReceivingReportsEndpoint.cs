using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.SuppliesAndMaterialsReceiving.List.v1;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.SuppliesAndMaterialsReceiving;

public static class ListSuppliesAndMaterialsReceivingReportsEndpoint
{
    internal static RouteHandlerBuilder MapListSuppliesAndMaterialsReceivingReportsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/list", async (ISender mediator) =>
            {
                var response = await mediator.Send(new ListSuppliesAndMaterialsReceivingReportsCommand());
                return Results.Ok(response);
            })
            .WithName(nameof(ListSuppliesAndMaterialsReceivingReportsEndpoint))
            .WithSummary("lists all supplies and materials receiving reports (SMRR)")
            .WithDescription("retrieves all supplies and materials receiving reports (SMRR) from inventory")
            .Produces<ListSuppliesAndMaterialsReceivingReportsResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .RequirePermission("Permissions.SuppliesAndMaterialsReceiving.View")
            .MapToApiVersion(new ApiVersion(1, 0));
    }
}
