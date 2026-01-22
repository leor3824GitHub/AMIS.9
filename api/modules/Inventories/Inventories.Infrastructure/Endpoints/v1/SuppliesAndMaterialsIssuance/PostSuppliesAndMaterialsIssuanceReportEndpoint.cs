using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.SuppliesAndMaterialsIssuance.Post.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.SuppliesAndMaterialsIssuance;

public static class PostSuppliesAndMaterialsIssuanceReportEndpoint
{
    internal static RouteHandlerBuilder MapPostSuppliesAndMaterialsIssuanceReportEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id}/post", async (Guid id, ISender mediator) =>
            {
                var command = new PostSuppliesAndMaterialsIssuanceReportCommand(id);
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(PostSuppliesAndMaterialsIssuanceReportEndpoint))
            .WithSummary("Post supplies and materials issuance report (SMIR)")
            .WithDescription("Posts a SMIR and updates semi-expendable inventory registry and transaction logs")
            .Produces<PostSuppliesAndMaterialsIssuanceReportResponse>()
            .Produces(404)
            .RequirePermission("Permissions.Inventories.Post")
            .MapToApiVersion(1);
    }
}
