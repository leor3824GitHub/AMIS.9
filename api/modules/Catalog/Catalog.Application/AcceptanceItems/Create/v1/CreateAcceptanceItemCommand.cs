using MediatR;
using System.ComponentModel;

namespace AMIS.WebApi.Catalog.Application.AcceptanceItems.Create.v1;

public sealed record CreateAcceptanceItemCommand(
    [property: DefaultValue("00000000-0000-0000-0000-000000000001")] Guid AcceptanceId,
    [property: DefaultValue("00000000-0000-0000-0000-000000000002")] Guid PurchaseItemId,
    [property: DefaultValue(10)] int QtyAccepted,
    [property: DefaultValue("Missing item")] string? Remarks
) : IRequest<CreateAcceptanceItemResponse>;
