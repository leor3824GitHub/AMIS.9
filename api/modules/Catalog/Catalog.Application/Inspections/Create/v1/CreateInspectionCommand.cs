﻿using System.ComponentModel;
using MediatR;

namespace AMIS.WebApi.Catalog.Application.Inspections.Create.v1;

public sealed record CreateInspectionCommand(
    DateTime InspectionDate,
    Guid InspectorId,
    Guid InspectionRequestId,
    Guid PurchaseId,
    string? Remarks,
    List<InspectionItemDto>? Items = null
) : IRequest<CreateInspectionResponse>;

public sealed record InspectionItemDto(
    Guid InspectionId,
    Guid PurchaseItemId,
    int QtyInspected,
    int QtyPassed,
    int QtyFailed,
    string? Remarks
);
