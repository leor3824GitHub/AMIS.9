using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.Services;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.AssetManagement;

public static class ReclassifyAssetsEndpoint
{
    /// <summary>
    /// Endpoint to trigger bulk asset reclassification when COA/DBM threshold changes
    /// Example: When threshold changes from ₱50,000 to ₱100,000
    /// Usage: POST /asset-management/reclassify
    /// Body: { "newPPEThreshold": 100000, "effectiveDate": "2026-01-01", "coaReference": "COA Circular 2025-XX", "reason": "Updated PPE threshold per COA" }
    /// </summary>
    internal static RouteHandlerBuilder MapReclassifyAssetsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/reclassify", async (ReclassifyAssetsCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(ReclassifyAssetsEndpoint))
            .WithSummary("Reclassify assets when COA/DBM threshold changes")
            .WithDescription(@"Bulk reclassifies assets when government accounting thresholds change. 
                Example: When PPE threshold increases from ₱50,000 to ₱100,000, assets with cost 
                between ₱50,001-₱100,000 are automatically reclassified from PPE to Semi-Expendable.
                This updates AssetClassificationRules, reclassifies affected PhysicalAssets, and 
                transfers entries from PPE registry to SEMEX registry with full audit trail.")
            .Produces<ReclassifyAssetsResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .RequirePermission("Permissions.Inventories.Update")
            .MapToApiVersion(new ApiVersion(1, 0));
    }
}
