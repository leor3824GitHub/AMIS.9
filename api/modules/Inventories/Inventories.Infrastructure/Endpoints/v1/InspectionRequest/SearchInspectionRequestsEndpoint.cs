using AMIS.Framework.Core.Paging;
using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.InspectionRequests.Get.v1;
using AMIS.WebApi.Inventories.Application.InspectionRequests.Search.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.InspectionRequest;

public static class SearchInspectionRequestsEndpoint
{
    internal static RouteHandlerBuilder MapGetInspectionRequestListEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (ISender mediator, [FromBody] SearchInspectionRequestsCommand command) =>
            {
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchInspectionRequestsEndpoint))
            .WithSummary("Gets a list of inspectionRequests")
            .WithDescription("Gets a list of inspectionRequests with pagination and filtering support")
            .Produces<PagedList<InspectionRequestResponse>>()
            .RequirePermission("Permissions.InspectionRequests.View")
            .MapToApiVersion(1);
    }
}


