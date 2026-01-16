using MediatR;

namespace AMIS.WebApi.Catalog.Application.JournalEntryVouchers.Submit.v1;

/// <summary>
/// Command to submit a journal entry voucher for approval
/// </summary>
public record SubmitJournalEntryVoucherCommand : IRequest<SubmitJournalEntryVoucherResponse>
{
    public Guid Id { get; init; }
}

/// <summary>
/// Response for submitting a journal entry voucher
/// </summary>
public record SubmitJournalEntryVoucherResponse
{
    public Guid Id { get; init; }
    public string Status { get; init; } = "Pending";
    public string Message { get; init; } = "JEV submitted for approval";
}
