using System.ComponentModel;
using AMIS.WebApi.Inventories.Domain.ValueObjects;
using MediatR;

namespace AMIS.WebApi.Inventories.Application.InspectionRequests.Create.v1;

public sealed record CreateInspectionRequestCommand(
    Guid? PurchaseId,
    Guid? InspectorId = null
) : IRequest<CreateInspectionRequestResponse>;



