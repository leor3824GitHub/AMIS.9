using MediatR;

namespace AMIS.WebApi.Inventories.Application.InspectionRequests.AssignInspector.v1;

public sealed record AssignInspectorToInspectionRequestCommand(Guid Id, Guid InspectorId) : IRequest<AssignInspectorToInspectionRequestResponse>;
public sealed record AssignInspectorToInspectionRequestResponse(Guid Id);

