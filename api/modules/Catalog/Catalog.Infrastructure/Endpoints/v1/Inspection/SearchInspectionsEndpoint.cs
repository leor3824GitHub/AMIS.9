using AMIS.Framework.Core.Paging;
using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.Inspections.Get.v1;
using AMIS.WebApi.Catalog.Application.Inspections.Search.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.Inspection.v1;

public static class SearchInspectionsEndpoint
{
    internal static RouteHandlerBuilder MapGetInspectionListEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (ISender mediator, [FromBody] SearchInspectionsCommand command) =>
            {
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchInspectionsEndpoint))
            .WithSummary("Gets a list of inspections")
            .WithDescription("Gets a list of inspections with pagination and filtering support")
            .Produces<PagedList<InspectionResponse>>()
            .RequirePermission("Permissions.Inspections.View")
            .MapToApiVersion(1);
    }
}

