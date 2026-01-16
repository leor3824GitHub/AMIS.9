using MediatR;

namespace AMIS.WebApi.Catalog.Application.JournalEntryVouchers.Get.v1;

public sealed record GetJournalEntryVoucherCommand(Guid Id) : IRequest<GetJournalEntryVoucherResponse>;

public sealed record GetJournalEntryVoucherResponse(
    Guid Id,
    string VoucherNumber,
    int Year,
    int Month,
    string Status,
    decimal TotalDebitAmount,
    decimal TotalCreditAmount,
    DateTime? PostedDate);
