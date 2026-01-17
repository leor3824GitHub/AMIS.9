using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Inventories.Application.JournalEntryVouchers.Export.v1;

public sealed class ExportJournalEntryVoucherHandler(
    [FromKeyedServices("inventories:journalentryvouchers")] IRepository<JournalEntryVoucher> repository)
    : IRequestHandler<ExportJournalEntryVoucherCommand, ExportJournalEntryVoucherResponse>
{
    public async Task<ExportJournalEntryVoucherResponse> Handle(
        ExportJournalEntryVoucherCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var journalEntryVoucher = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new InvalidOperationException($"Journal entry voucher {request.Id} not found");

        // Update export information
        if (!string.IsNullOrWhiteSpace(request.ExportFormat))
        {
            journalEntryVoucher.GetType().GetProperty(nameof(JournalEntryVoucher.ExportFormat))?
                .SetValue(journalEntryVoucher, request.ExportFormat);
        }

        var exportedDate = DateTime.UtcNow;
        journalEntryVoucher.GetType().GetProperty(nameof(JournalEntryVoucher.ExportedDate))?
            .SetValue(journalEntryVoucher, exportedDate);

        // Generate export file name
        var fileName = $"JEV-{journalEntryVoucher.VoucherNumber}-{DateTime.UtcNow:yyyyMMdd-HHmmss}.{request.ExportFormat.ToLower()}";
        journalEntryVoucher.GetType().GetProperty(nameof(JournalEntryVoucher.ExportFileName))?
            .SetValue(journalEntryVoucher, fileName);

        await repository.UpdateAsync(journalEntryVoucher, cancellationToken);

        return new ExportJournalEntryVoucherResponse(
            journalEntryVoucher.Id,
            journalEntryVoucher.ExportFormat ?? string.Empty,
            journalEntryVoucher.ExportedDate ?? DateTime.UtcNow,
            journalEntryVoucher.ExportFileName);
    }
}

