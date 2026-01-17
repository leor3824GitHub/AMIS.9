using AMIS.WebApi.Inventories.Domain.ValueObjects;
using MediatR;

namespace AMIS.WebApi.Inventories.Application.InspectionRequests.UpdateStatus.v1;

public sealed record UpdateInspectionRequestStatusCommand(Guid Id, InspectionRequestStatus Status) : IRequest<UpdateInspectionRequestStatusResponse>;
public sealed record UpdateInspectionRequestStatusResponse(Guid Id);

