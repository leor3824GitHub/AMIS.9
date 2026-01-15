using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.JournalEntryVouchers.Export.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1.JournalEntryVoucher;

public static class ExportJournalEntryVoucherEndpoint
{
    internal static RouteHandlerBuilder MapExportJournalEntryVoucherEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/export", ExportHandler)
            .WithName(nameof(ExportJournalEntryVoucherEndpoint))
            .Produces<ExportJournalEntryVoucherResponse>()
            .RequirePermission("Permissions.JournalEntryVouchers.View")
            .MapToApiVersion(1);
    }

    private static async Task<IResult> ExportHandler(
        Guid id,
        [FromBody] ExportRequest request,
        ISender mediator,
        CancellationToken cancellationToken = default)
    {
        var command = new ExportJournalEntryVoucherCommand(id, request.ExportFormat);
        var response = await mediator.Send(command, cancellationToken);
        return Results.Ok(response);
    }
}

public sealed record ExportRequest(string ExportFormat);
