using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.Acceptances.LinkInspection.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;

public static class LinkAcceptanceInspectionEndpoint
{
    internal static RouteHandlerBuilder MapAcceptanceLinkInspectionEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{acceptanceId:guid}/link-inspection/{inspectionId:guid}", async (Guid acceptanceId, Guid inspectionId, ISender mediator) =>
            {
                var response = await mediator.Send(new LinkAcceptanceInspectionCommand(acceptanceId, inspectionId));
                return Results.Ok(response);
            })
            .WithName(nameof(LinkAcceptanceInspectionEndpoint))
            .WithSummary("link acceptance to inspection")
            .WithDescription("link an acceptance record to an approved inspection")
            .Produces<LinkAcceptanceInspectionResponse>()
            .RequirePermission("Permissions.Acceptances.Link")
            .MapToApiVersion(1);
    }
}
