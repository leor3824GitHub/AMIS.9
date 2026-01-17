using AMIS.Framework.Core.Paging;
using AMIS.WebApi.Inventories.Domain;
using MediatR;

namespace AMIS.WebApi.Inventories.Application.JournalEntryVouchers.Search.v1;

public sealed record SearchJournalEntryVouchersCommand(
    int PageNumber = 1,
    int PageSize = 10) : IRequest<PagedList<JournalEntryVoucherDto>>;

public sealed record JournalEntryVoucherDto(
    Guid Id,
    string VoucherNumber,
    int Year,
    int Month,
    string Status,
    decimal TotalDebitAmount,
    decimal TotalCreditAmount,
    DateTime? PostedDate);

