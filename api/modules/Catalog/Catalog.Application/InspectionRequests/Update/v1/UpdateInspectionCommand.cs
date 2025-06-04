using MediatR;
using System.ComponentModel;

namespace AMIS.WebApi.Catalog.Application.Inspections.Update.v1;

public sealed record UpdateInspectionCommand(
    Guid Id,
    DateTime InspectionDate,
    Guid InspectedBy,
    Guid PurchaseId,
    string? Remarks
) : IRequest<UpdateInspectionResponse>;
