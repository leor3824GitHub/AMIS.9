using System.ComponentModel;
using MediatR;
using AMIS.WebApi.Inventories.Domain;
using AMIS.WebApi.Inventories.Domain.ValueObjects;

namespace AMIS.WebApi.Inventories.Application.Inspections.Create.v1;

/// <summary>
/// Creates a new inspection for one of three scenarios: NewDelivery, AssetReturn, or Repair
/// </summary>
public sealed record CreateInspectionCommand(
    InspectionType Type,
    Guid EmployeeId,
    DateTime? InspectedOn = null,
    Guid? PurchaseId = null,
    Guid? PhysicalAssetId = null,
    string? Remarks = null,
    string? IARDocumentPath = null,
    List<InspectionItemDto>? Items = null
) : IRequest<CreateInspectionResponse>;

public sealed record InspectionItemDto(
    Guid PurchaseItemId,
    int QtyInspected,
    int QtyPassed,
    int QtyFailed,
    string? Remarks,
    [property: DefaultValue(InspectionItemStatus.NotInspected)] InspectionItemStatus? InspectionItemStatus
);

