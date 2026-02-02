using MediatR;

namespace AMIS.WebApi.Inventories.Application.Disposals.Complete.v1;

/// <summary>
/// Command to complete an approved disposal
/// Records actual salvage value and financial impact
/// </summary>
public sealed record CompleteDisposalCommand(
    Guid DisposalId,
    decimal? SalvageValue = null,
    string? DisposalReferenceNumber = null) : IRequest<CompleteDisposalResponse>;

public sealed record CompleteDisposalResponse(
    Guid DisposalId,
    string PropertyCode,
    string Status,
    DateTime CompletedOn,
    decimal? SalvageValue,
    decimal? GainOrLoss);
