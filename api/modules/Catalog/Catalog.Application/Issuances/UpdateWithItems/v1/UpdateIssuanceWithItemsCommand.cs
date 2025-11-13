using MediatR;

namespace AMIS.WebApi.Catalog.Application.Issuances.UpdateWithItems.v1;

public sealed record UpdateIssuanceWithItemsCommand(
    Guid Id,
    Guid EmployeeId,
    DateTime IssuanceDate,
    decimal TotalAmount,
    bool IsClosed,
    IReadOnlyList<IssuanceItemUpsert> Items,
    IReadOnlyList<Guid>? DeletedItemIds
) : IRequest<UpdateIssuanceWithItemsResponse>;