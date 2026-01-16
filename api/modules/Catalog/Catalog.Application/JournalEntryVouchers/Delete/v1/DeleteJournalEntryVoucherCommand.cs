using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.JournalEntryVouchers.Delete.v1;

public sealed record DeleteJournalEntryVoucherCommand(Guid Id) : IRequest<DeleteJournalEntryVoucherResponse>;

public sealed record DeleteJournalEntryVoucherResponse(Guid Id);

public sealed class DeleteJournalEntryVoucherHandler(
    ILogger<DeleteJournalEntryVoucherHandler> logger,
    [FromKeyedServices("catalog:journalentryv ouchers")] IRepository<JournalEntryVoucher> repository)
    : IRequestHandler<DeleteJournalEntryVoucherCommand, DeleteJournalEntryVoucherResponse>
{
    public async Task<DeleteJournalEntryVoucherResponse> Handle(DeleteJournalEntryVoucherCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var voucher = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new InvalidOperationException($"Journal entry voucher {request.Id} not found");

        await repository.DeleteAsync(voucher, cancellationToken);

        logger.LogInformation("Journal entry voucher {VoucherId} deleted", voucher.Id);
        return new DeleteJournalEntryVoucherResponse(voucher.Id);
    }
}
