using MediatR;
using AMIS.WebApi.Catalog.Domain.ValueObjects;

namespace AMIS.WebApi.Catalog.Application.Inspections.ManageItems.v1;

public sealed record UpdateInspectionItemCommand(
    Guid InspectionId,
    Guid ItemId,
    int QtyInspected,
    int QtyPassed,
    int QtyFailed,
    string? Remarks,
    InspectionItemStatus? InspectionItemStatus
) : IRequest;
