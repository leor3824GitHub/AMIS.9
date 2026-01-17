using System.ComponentModel;
using AMIS.WebApi.Inventories.Domain.ValueObjects;
using MediatR;

namespace AMIS.WebApi.Inventories.Application.Inspections.ManageItems.v1;

public sealed record AddInspectionItemCommand(
    Guid InspectionId,
    Guid PurchaseItemId,
    int QtyInspected,
    int QtyPassed,
    int QtyFailed,
    string? Remarks,
    InspectionItemStatus? InspectionItemStatus
) : IRequest<AddInspectionItemResponse>;

