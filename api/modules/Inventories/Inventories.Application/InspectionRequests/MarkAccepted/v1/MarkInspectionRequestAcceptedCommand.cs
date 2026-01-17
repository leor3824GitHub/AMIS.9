using MediatR;

namespace AMIS.WebApi.Inventories.Application.InspectionRequests.MarkAccepted.v1;

public sealed record MarkInspectionRequestAcceptedCommand(Guid Id) : IRequest<MarkInspectionRequestAcceptedResponse>;
public sealed record MarkInspectionRequestAcceptedResponse(Guid Id);

