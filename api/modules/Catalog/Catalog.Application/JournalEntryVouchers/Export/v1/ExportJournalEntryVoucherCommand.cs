using MediatR;

namespace AMIS.WebApi.Catalog.Application.JournalEntryVouchers.Export.v1;

public sealed record ExportJournalEntryVoucherCommand(
    Guid Id,
    string ExportFormat) : IRequest<ExportJournalEntryVoucherResponse>;

public sealed record ExportJournalEntryVoucherResponse(
    Guid Id,
    string ExportFormat,
    DateTime ExportedDate,
    string? ExportFileName);
