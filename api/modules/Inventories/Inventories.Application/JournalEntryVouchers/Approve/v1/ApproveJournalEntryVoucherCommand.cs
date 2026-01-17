using MediatR;

namespace AMIS.WebApi.Inventories.Application.JournalEntryVouchers.Approve.v1;

/// <summary>
/// Command to approve a journal entry voucher
/// </summary>
public record ApproveJournalEntryVoucherCommand : IRequest<ApproveJournalEntryVoucherResponse>
{
    public Guid Id { get; init; }
}

/// <summary>
/// Response for approving a journal entry voucher
/// </summary>
public record ApproveJournalEntryVoucherResponse
{
    public Guid Id { get; init; }
    public string Status { get; init; } = "Posted";
    public string Message { get; init; } = "JEV approved and posted to ledger";
}

