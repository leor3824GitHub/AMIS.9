using AMIS.Framework.Core.Domain.Events;

namespace AMIS.WebApi.Catalog.Domain.Events;

// Journal Entry Voucher Events
public sealed record JournalEntryVoucherCreated : DomainEvent
{
    public JournalEntryVoucher JournalEntryVoucher { get; set; } = default!;
}

public sealed record JournalEntryVoucherUpdated : DomainEvent
{
    public JournalEntryVoucher JournalEntryVoucher { get; set; } = default!;
}

public sealed record JournalEntryVoucherPosted : DomainEvent
{
    public JournalEntryVoucher JournalEntryVoucher { get; set; } = default!;
}
