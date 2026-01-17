using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.JournalEntryVouchers.Update.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.JournalEntryVoucher;

public static class UpdateJournalEntryVoucherEndpoint
{
    internal static RouteHandlerBuilder MapUpdateJournalEntryVoucherEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", UpdateHandler)
            .WithName(nameof(UpdateJournalEntryVoucherEndpoint))
            .Produces<UpdateJournalEntryVoucherResponse>()
            .RequirePermission("Permissions.JournalEntryVouchers.Edit")
            .MapToApiVersion(1);
    }

    private static async Task<IResult> UpdateHandler(
        Guid id,
        UpdateJournalEntryVoucherCommand command,
        ISender mediator,
        CancellationToken cancellationToken = default)
    {
        var updateCommand = new UpdateJournalEntryVoucherCommand(id, command.ExportFormat, command.Remarks);
        var response = await mediator.Send(updateCommand, cancellationToken);
        return Results.Ok(response);
    }
}

