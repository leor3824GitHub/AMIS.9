using MediatR;

namespace AMIS.WebApi.Catalog.Application.Acceptances.ManageItems.v1;

public sealed record UpdateAcceptanceItemCommand(
    Guid AcceptanceId,
    Guid ItemId,
    int QtyAccepted,
    string? Remarks
) : IRequest;
