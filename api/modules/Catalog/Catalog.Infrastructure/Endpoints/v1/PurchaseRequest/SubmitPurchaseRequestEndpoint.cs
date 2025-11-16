using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.PurchaseRequests.Status.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;
public static class SubmitPurchaseRequestEndpoint
{
    internal static RouteHandlerBuilder MapPurchaseRequestSubmitEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/submit", async (Guid id, ISender mediator) =>
            {
                await mediator.Send(new SubmitPurchaseRequestCommand(id));
                return Results.NoContent();
            })
            .WithName(nameof(SubmitPurchaseRequestEndpoint))
            .WithSummary("submit purchase request")
            .WithDescription("submit a draft purchase request")
            .RequirePermission("Permissions.PurchaseRequests.Update")
            .MapToApiVersion(1);
    }
}
