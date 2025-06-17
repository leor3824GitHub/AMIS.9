using System.ComponentModel;
using AMIS.WebApi.Catalog.Domain.ValueObjects;
using MediatR;

namespace AMIS.WebApi.Catalog.Application.InspectionRequests.Create.v1;

public sealed record CreateInspectionRequestCommand(
    Guid? PurchaseId,
    Guid RequestedById,
    Guid? AssignedInspectorId = null
) : IRequest<CreateInspectionRequestResponse>;


