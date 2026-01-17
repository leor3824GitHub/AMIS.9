using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.DepreciationSchedules.Update.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.DepreciationSchedule;

public static class UpdateDepreciationScheduleEndpoint
{
    internal static RouteHandlerBuilder MapUpdateDepreciationScheduleEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", UpdateHandler)
            .WithName(nameof(UpdateDepreciationScheduleEndpoint))
            .Produces<UpdateDepreciationScheduleResponse>()
            .RequirePermission("Permissions.DepreciationSchedules.Edit")
            .MapToApiVersion(1);
    }

    private static async Task<IResult> UpdateHandler(
        Guid id,
        UpdateDepreciationScheduleCommand command,
        ISender mediator,
        CancellationToken cancellationToken = default)
    {
        var updateCommand = new UpdateDepreciationScheduleCommand(id, command.Remarks);
        var response = await mediator.Send(updateCommand, cancellationToken);
        return Results.Ok(response);
    }
}

