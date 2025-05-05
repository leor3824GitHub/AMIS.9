using AMIS.Framework.Core.Paging;
using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.InspectionItems.Get.v1;
using AMIS.WebApi.Catalog.Application.InspectionItems.Search.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;

public static class SearchInspectionItemsEndpoint
{
    internal static RouteHandlerBuilder MapGetInspectionItemListEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (ISender mediator, [FromBody] SearchInspectionItemsCommand command) =>
            {
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchInspectionItemsEndpoint))
            .WithSummary("Gets a list of inspections")
            .WithDescription("Gets a list of inspections with pagination and filtering support")
            .Produces<PagedList<InspectionItemResponse>>()
            .RequirePermission("Permissions.InspectionItems.View")
            .MapToApiVersion(1);
    }
}

