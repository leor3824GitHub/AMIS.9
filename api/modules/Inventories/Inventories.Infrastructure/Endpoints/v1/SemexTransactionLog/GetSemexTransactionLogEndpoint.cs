using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.SemexTransactionLog.Get.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.SemexTransactionLog;

public static class GetSemexTransactionLogEndpoint
{
    internal static RouteHandlerBuilder MapGetSemexTransactionLogEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id}", async (Guid id, ISender mediator) =>
            {
                var query = new GetSemexTransactionLogByIdQuery(id);
                var response = await mediator.Send(query);
                return Results.Ok(response);
            })
            .WithName(nameof(GetSemexTransactionLogEndpoint))
            .WithSummary("Gets a semi-expendable inventory transaction log entry by id")
            .WithDescription("Gets a semi-expendable inventory transaction log entry by id")
            .Produces<SemexTransactionLogDetailResponse>()
            .Produces(404)
            .RequirePermission("Permissions.InventoryTransactions.View")
            .MapToApiVersion(1);
    }
}
