using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.Categories.Update.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.Category;
public static class UpdateCategoryEndpoint
{
    internal static RouteHandlerBuilder MapCategoryUpdateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (Guid id, UpdateCategoryCommand request, ISender mediator) =>
            {
                if (id != request.Id) return Results.BadRequest();
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdateCategoryEndpoint))
            .WithSummary("update a category")
            .WithDescription("update a category")
            .Produces<UpdateCategoryResponse>()
            .RequirePermission("Permissions.Categories.Update")
            .MapToApiVersion(1);
    }
}

