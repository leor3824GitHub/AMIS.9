﻿using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.Purchases.Delete.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;
public static class DeletePurchaseEndpoint
{
    internal static RouteHandlerBuilder MapPurchaseDeleteEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id:guid}", async (Guid id, ISender mediator) =>
             {
                 await mediator.Send(new DeletePurchaseCommand(id));
                 return Results.NoContent();
             })
            .WithName(nameof(DeletePurchaseEndpoint))
            .WithSummary("deletes purchase by id")
            .WithDescription("deletes purchase by id")
            .Produces(StatusCodes.Status204NoContent)
            .RequirePermission("Permissions.Purchases.Delete")
            .MapToApiVersion(1);
    }
}
