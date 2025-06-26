using AMIS.WebApi.Catalog.Domain.ValueObjects;
using MediatR;
using System.ComponentModel;

namespace AMIS.WebApi.Catalog.Application.InspectionRequests.Update.v1;

public sealed record UpdateInspectionRequestCommand(
   Guid Id,
   Guid? PurchaseId,
   Guid? InspectorId
) : IRequest<UpdateInspectionRequestResponse>;
