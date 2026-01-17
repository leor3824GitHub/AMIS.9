using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.DepreciationSchedules.Create.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.DepreciationSchedule;

public static class CreateDepreciationScheduleEndpoint
{
    internal static RouteHandlerBuilder MapDepreciationScheduleCreationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateDepreciationScheduleCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(CreateDepreciationScheduleEndpoint))
            .WithSummary("Creates a depreciation schedule")
            .WithDescription("Creates a monthly depreciation schedule for a physical asset")
            .Produces<CreateDepreciationScheduleResponse>()
            .RequirePermission("Permissions.DepreciationSchedules.Create")
            .MapToApiVersion(1);
    }
}

