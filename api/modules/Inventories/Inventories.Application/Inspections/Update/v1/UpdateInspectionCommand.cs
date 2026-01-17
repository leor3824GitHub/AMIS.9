using MediatR;
using System.ComponentModel;

namespace AMIS.WebApi.Inventories.Application.Inspections.Update.v1;

public sealed record UpdateInspectionCommand(
    Guid Id,
    DateTime InspectionDate,
    Guid? InspectorId,
    Guid? InspectionRequestId,
    string? Remarks
) : IRequest<UpdateInspectionResponse>;

