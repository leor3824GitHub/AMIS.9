using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Catalog.Application.JournalEntryVouchers.Update.v1;

public sealed class UpdateJournalEntryVoucherHandler(
    [FromKeyedServices("catalog:journalentryvouchers")] IRepository<JournalEntryVoucher> repository)
    : IRequestHandler<UpdateJournalEntryVoucherCommand, UpdateJournalEntryVoucherResponse>
{
    public async Task<UpdateJournalEntryVoucherResponse> Handle(
        UpdateJournalEntryVoucherCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var journalEntryVoucher = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new InvalidOperationException($"Journal entry voucher {request.Id} not found");

        // Only update export format and remarks
        if (!string.IsNullOrWhiteSpace(request.ExportFormat))
        {
            journalEntryVoucher.GetType().GetProperty(nameof(JournalEntryVoucher.ExportFormat))?
                .SetValue(journalEntryVoucher, request.ExportFormat);
        }

        if (!string.IsNullOrWhiteSpace(request.Remarks))
        {
            journalEntryVoucher.GetType().GetProperty(nameof(JournalEntryVoucher.Remarks))?
                .SetValue(journalEntryVoucher, request.Remarks);
        }

        await repository.UpdateAsync(journalEntryVoucher, cancellationToken);

        return new UpdateJournalEntryVoucherResponse(
            journalEntryVoucher.Id,
            journalEntryVoucher.ExportFormat,
            journalEntryVoucher.Remarks);
    }
}
