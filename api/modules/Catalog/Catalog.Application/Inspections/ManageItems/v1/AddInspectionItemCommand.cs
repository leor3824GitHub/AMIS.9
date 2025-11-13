using System.ComponentModel;
using AMIS.WebApi.Catalog.Domain.ValueObjects;
using MediatR;

namespace AMIS.WebApi.Catalog.Application.Inspections.ManageItems.v1;

public sealed record AddInspectionItemCommand(
    Guid InspectionId,
    Guid PurchaseItemId,
    int QtyInspected,
    int QtyPassed,
    int QtyFailed,
    string? Remarks,
    InspectionItemStatus? InspectionItemStatus
) : IRequest<AddInspectionItemResponse>;
