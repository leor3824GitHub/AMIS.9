using AMIS.WebApi.Catalog.Domain.ValueObjects;
using MediatR;

namespace AMIS.WebApi.Catalog.Application.InspectionRequests.UpdateStatus.v1;

public sealed record UpdateInspectionRequestStatusCommand(Guid Id, InspectionRequestStatus Status) : IRequest<UpdateInspectionRequestStatusResponse>;
public sealed record UpdateInspectionRequestStatusResponse(Guid Id);
