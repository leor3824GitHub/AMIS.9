using MediatR;

namespace AMIS.WebApi.Inventories.Application.JournalEntryVouchers.Create.v1;

public sealed record CreateJournalEntryVoucherCommand(
    int Year,
    int Month) : IRequest<CreateJournalEntryVoucherResponse>;

public sealed record CreateJournalEntryVoucherResponse(Guid Id);

