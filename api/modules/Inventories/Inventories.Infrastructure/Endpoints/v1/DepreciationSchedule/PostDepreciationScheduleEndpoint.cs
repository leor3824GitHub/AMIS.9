using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.DepreciationSchedules.Post.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.DepreciationSchedule;

public static class PostDepreciationScheduleEndpoint
{
    internal static RouteHandlerBuilder MapDepreciationSchedulePostingEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}/post", async (Guid id, PostDepreciationScheduleCommand command, ISender mediator) =>
            {
                var response = await mediator.Send(command with { Id = id });
                return Results.Ok(response);
            })
            .WithName(nameof(PostDepreciationScheduleEndpoint))
            .WithSummary("Posts a depreciation schedule")
            .WithDescription("Posts a depreciation schedule to a journal entry voucher")
            .Produces<PostDepreciationScheduleResponse>()
            .RequirePermission("Permissions.DepreciationSchedules.Post")
            .MapToApiVersion(1);
    }
}

