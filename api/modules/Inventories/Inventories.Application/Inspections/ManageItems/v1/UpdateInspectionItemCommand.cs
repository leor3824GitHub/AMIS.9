using MediatR;
using AMIS.WebApi.Inventories.Domain.ValueObjects;

namespace AMIS.WebApi.Inventories.Application.Inspections.ManageItems.v1;

public sealed record UpdateInspectionItemCommand(
    Guid InspectionId,
    Guid ItemId,
    int QtyInspected,
    int QtyPassed,
    int QtyFailed,
    string? Remarks,
    InspectionItemStatus? InspectionItemStatus
) : IRequest;

