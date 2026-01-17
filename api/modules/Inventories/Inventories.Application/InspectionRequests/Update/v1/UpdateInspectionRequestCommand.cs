using AMIS.WebApi.Inventories.Domain.ValueObjects;
using MediatR;
using System.ComponentModel;

namespace AMIS.WebApi.Inventories.Application.InspectionRequests.Update.v1;

public sealed record UpdateInspectionRequestCommand(
   Guid Id,
   Guid? PurchaseId,
   Guid? InspectorId,
   InspectionRequestStatus Status
) : IRequest<UpdateInspectionRequestResponse>;

