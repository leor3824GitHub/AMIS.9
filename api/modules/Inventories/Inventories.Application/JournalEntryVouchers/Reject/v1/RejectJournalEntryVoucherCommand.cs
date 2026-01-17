using MediatR;

namespace AMIS.WebApi.Inventories.Application.JournalEntryVouchers.Reject.v1;

/// <summary>
/// Command to reject a journal entry voucher
/// </summary>
public record RejectJournalEntryVoucherCommand : IRequest<RejectJournalEntryVoucherResponse>
{
    public Guid Id { get; init; }
    public string Reason { get; init; } = string.Empty;
}

/// <summary>
/// Response for rejecting a journal entry voucher
/// </summary>
public record RejectJournalEntryVoucherResponse
{
    public Guid Id { get; init; }
    public string Status { get; init; } = "Rejected";
    public string? Reason { get; init; }
    public string Message { get; init; } = "JEV rejected";
}

