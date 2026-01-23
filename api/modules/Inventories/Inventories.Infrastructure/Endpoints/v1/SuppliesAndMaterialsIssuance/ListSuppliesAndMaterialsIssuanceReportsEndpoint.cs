using Asp.Versioning;
using MediatR;
using AMIS.WebApi.Inventories.Application.SuppliesAndMaterialsIssuance.List.v1;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.SuppliesAndMaterialsIssuance;

public static class ListSuppliesAndMaterialsIssuanceReportsEndpoint
{
    internal static RouteHandlerBuilder MapListSuppliesAndMaterialsIssuanceReportsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/list", async (ISender mediator) =>
            {
                var response = await mediator.Send(new ListSuppliesAndMaterialsIssuanceReportsCommand());
                return Results.Ok(response);
            })
            .WithName(nameof(ListSuppliesAndMaterialsIssuanceReportsEndpoint))
            .WithSummary("lists all supplies and materials issuance reports (SMIR)")
            .WithDescription("retrieves all supplies and materials issuance reports (SMIR) from inventory")
            .Produces<ListSuppliesAndMaterialsIssuanceReportsResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .RequirePermission("Permissions.SuppliesAndMaterialsIssuance.View")
            .MapToApiVersion(new ApiVersion(1, 0));
    }
}
