using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.SuppliesAndMaterialsReceiving.Post.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.SuppliesAndMaterialsReceiving;

public static class PostSuppliesAndMaterialsReceivingReportEndpoint
{
    internal static RouteHandlerBuilder MapPostSuppliesAndMaterialsReceivingReportEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id}/post", async (Guid id, ISender mediator) =>
            {
                var command = new PostSuppliesAndMaterialsReceivingReportCommand(id);
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(PostSuppliesAndMaterialsReceivingReportEndpoint))
            .WithSummary("Post supplies and materials receiving report (SMRR)")
            .WithDescription("Posts an SMRR and updates semi-expendable inventory registry and transaction logs")
            .Produces<PostSuppliesAndMaterialsReceivingReportResponse>()
            .Produces(404)
            .RequirePermission("Permissions.Inventories.Post")
            .MapToApiVersion(1);
    }
}
