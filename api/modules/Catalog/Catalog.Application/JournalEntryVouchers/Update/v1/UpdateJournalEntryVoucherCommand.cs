using MediatR;

namespace AMIS.WebApi.Catalog.Application.JournalEntryVouchers.Update.v1;

public sealed record UpdateJournalEntryVoucherCommand(
    Guid Id,
    string? ExportFormat = null,
    string? Remarks = null) : IRequest<UpdateJournalEntryVoucherResponse>;

public sealed record UpdateJournalEntryVoucherResponse(
    Guid Id,
    string? ExportFormat,
    string? Remarks);
