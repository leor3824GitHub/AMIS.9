using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.JournalEntryVouchers.Delete.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.JournalEntryVoucher;

public static class DeleteJournalEntryVoucherEndpoint
{
    internal static RouteHandlerBuilder MapDeleteJournalEntryVoucherEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id:guid}", DeleteHandler)
            .WithName(nameof(DeleteJournalEntryVoucherEndpoint))
            .Produces<DeleteJournalEntryVoucherResponse>()
            .RequirePermission("Permissions.JournalEntryVouchers.View")
            .MapToApiVersion(1);
    }

    private static async Task<IResult> DeleteHandler(
        Guid id,
        ISender mediator,
        CancellationToken cancellationToken = default)
    {
        var command = new DeleteJournalEntryVoucherCommand(id);
        var response = await mediator.Send(command, cancellationToken);
        return Results.Ok(response);
    }
}

